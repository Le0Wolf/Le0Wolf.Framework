// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Parser.Impl
{
    #region usings

    using System.Text.RegularExpressions;

    using Exceptions;

    #endregion

    /// <summary>
    ///     Парсер шаблона
    /// </summary>
    public class TokensVisitor : ITokensVisitor
    {
        private enum State
        {
            ReadLiteral,
            ReadVariable,
            VariableReadEnd
        }

        private static readonly Regex VariableValidationRegex
            = new("^[A-z0-9_]+$", RegexOptions.Compiled);

        private readonly ITemplateVisitor templateVisitor;
        private State currentState = State.ReadLiteral;

        public TokensVisitor(ITemplateVisitor templateVisitor)
        {
            this.templateVisitor = templateVisitor;
        }

        /// <summary>
        ///     Обрабатывает литерал
        /// </summary>
        /// <param name="position"></param>
        /// <param name="literalValue"></param>
        public void VisitLiteralToken(Position position, string literalValue)
        {
            switch (this.currentState)
            {
                case State.ReadLiteral:
                    this.templateVisitor.VisitLiteral(literalValue);

                    break;
                case State.ReadVariable:
                    if (!this.IsValidVariableName(literalValue))
                    {
                        throw new TemplateValidationException(position,
                            "Имя переменной имеет неверный формат. Оно может содержать только строчные и заглавные буквы английского алфавита, а так же цифры и символ подчёркивания.",
                            literalValue) {Code = 0};
                    }

                    this.templateVisitor.VisitVariable(literalValue);
                    this.currentState = State.VariableReadEnd;

                    break;
                case State.VariableReadEnd:
                    throw new TemplateParserException(position,
                        $"Неожиданный символ \"{literalValue}\". Ожидалось \"}}}}\".")
                    {
                        Code = 1
                    };
                default:
                    throw new TemplateParserException(position,
                        "Некорректное состояние парсера") {Code = 3};
            }
        }

        /// <summary>
        ///     Обрабатывает открывающую фигурную скобку
        /// </summary>
        /// <param name="position"></param>
        public void VisitOpenBrackedToken(Position position)
        {
            this.currentState = this.currentState switch
            {
                State.ReadLiteral => State.ReadVariable,
                State.ReadVariable => throw new TemplateParserException(
                    position,
                    "Неожиданный символ \"{{\". Ожидалось имя переменной.")
                {
                    Code = 4
                },
                State.VariableReadEnd => throw new TemplateParserException(
                    position, "Неожиданный символ \"{{\". Ожидалось \"}}\".")
                {
                    Code = 5
                },
                _ => throw new TemplateParserException(position,
                    "Некорректное состояние парсера") {Code = 3}
            };
        }

        /// <summary>
        ///     Обрабатывает закрывающую фигурную скобку
        /// </summary>
        /// <param name="position"></param>
        public void VisitCloseBrackedToken(Position position)
        {
            this.currentState = this.currentState switch
            {
                State.ReadLiteral => throw new TemplateParserException(position,
                    "Неожиданный символ \"}}\". Ожидалось \"{{\".") {Code = 6},
                State.ReadVariable => throw new TemplateParserException(
                    position,
                    "Неожиданный символ \"}}\". Ожидалось имя переменной.")
                {
                    Code = 7
                },
                State.VariableReadEnd => State.ReadLiteral,
                _ => throw new TemplateParserException(position,
                    "Некорректное состояние парсера") {Code = 3}
            };
        }

        /// <summary>
        ///     Обрабатывает событие завершения обработки
        /// </summary>
        public void OnEnd()
        {
            switch (this.currentState)
            {
                case State.ReadLiteral:
                    break;
                case State.ReadVariable:
                    throw new TemplateParserException(
                        "Неожиданный конец шаблона. Ожидалось имя переменной.")
                    {
                        Code = 8
                    };
                case State.VariableReadEnd:
                    throw new TemplateParserException(
                        "Неожиданный конец шаблона. Ожидалось \"}}\".")
                    {
                        Code = 9
                    };
                default:
                    throw new TemplateParserException(
                        "Некорректное состояние парсера по завершению обработки")
                    {
                        Code = 3
                    };
            }
        }

        private bool IsValidVariableName(string name)
        {
            return VariableValidationRegex.IsMatch(name);
        }
    }
}