// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Impl
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;

    using Infrastructure;

    using Parser;

    #endregion

    /// <summary>
    ///     Компилирует шаблон путем обхода его текста в виде визитора
    /// </summary>
    public class TemplateCompiller : ITemplateCompiler, ITemplateVisitor
    {
        private readonly ITemplateParser templateParser;

        private ParameterExpression inputObjParamExpr;
        private ParameterExpression stringBuilderObjExpr;
        private List<Expression> body;

        public TemplateCompiller(ITemplateParser templateParser)
        {
            this.templateParser = templateParser;
        }

        /// <summary>
        ///     Добавляет в дерево выражений конструкцию вида sb.Append(value) при посещении простой строки
        /// </summary>
        /// <param name="value">Текст строки</param>
        void ITemplateVisitor.VisitLiteral(string value)
        {
            var valueConstExpr = Expression.Constant(value);
            var resultExpr = this.BuildAppendExpression(valueConstExpr);
            this.body.Add(resultExpr);
        }

        /// <summary>
        ///     Добавляет в дерево выражений конструкцию вида sb.Append(obj.name) при посещении имени переменной
        /// </summary>
        /// <param name="name"></param>
        void ITemplateVisitor.VisitVariable(string name)
        {
            var valuePropertyExpr
                = Expression.Property(this.inputObjParamExpr, name);
            var resultExpr = this.BuildAppendExpression(valuePropertyExpr);
            this.body.Add(resultExpr);
        }

        /// <summary>
        ///     компилирует переданный текст шаблона
        /// </summary>
        /// <typeparam name="TTemplateArgs">Тип аргументов шаблона</typeparam>
        /// <param name="templateText">Текст шаблона</param>
        /// <returns>Делегат, заполняющий переменные шаблона из свойств переданного объекта</returns>
        public Func<TTemplateArgs, string> Compile<TTemplateArgs>(
            string templateText)
        {
            Func<TTemplateArgs, string> result;
            try
            {
                this.Prolog(typeof(TTemplateArgs));

                this.templateParser.Parse(templateText, this);

                var lambda = this.Epilog<TTemplateArgs>();

                result = lambda.Compile();
            }
            finally
            {
                this.body = null;
                this.inputObjParamExpr = null;
                this.stringBuilderObjExpr = null;
            }

            return result;
        }

        /// <summary>
        ///     Подгототавливает дерево выражений к дальнейшей обработке визитором
        /// </summary>
        /// <param name="inputType">Тип объекта, содержащего значения переменных шаблона</param>
        private void Prolog(Type inputType)
        {
            this.inputObjParamExpr = Expression.Parameter(inputType, "obj");
            this.stringBuilderObjExpr
                = Expression.Variable(typeof(StringBuilder), "sb");

            this.body = new List<Expression>
            {
                Expression.Assign(this.stringBuilderObjExpr,
                    Expression.New(typeof(StringBuilder)))
            };
        }

        /// <summary>
        ///     Завершает формирование дерева выражений
        /// </summary>
        /// <typeparam name="TTemplateArgs">Тип объекта, содержащего значения переменных шаблона</typeparam>
        /// <returns>Готовое к компиляции дерево выражений</returns>
        private Expression<Func<TTemplateArgs, string>> Epilog<TTemplateArgs>()
        {
            this.body.Add(this.BuildToStringExpression());
            var blockExpr = Expression.Block(new[]
            {
                this.stringBuilderObjExpr
            }, this.body);

            return Expression.Lambda<Func<TTemplateArgs, string>>(blockExpr,
                this.inputObjParamExpr);
        }

        /// <summary>
        ///     Формирут выражение добавления значения в StringBuilder (т.е. sb.Append(value)
        /// </summary>
        /// <param name="valueExpr">Дерево выражений, содержащее добавляемое значение</param>
        /// <returns>Дерево выражений, вызвающее добавление переданного значения в StringBuilder</returns>
        private Expression BuildAppendExpression(Expression valueExpr)
        {
            var mi = typeof(StringBuilder).GetMethod(
                nameof(StringBuilder.Append), new[]
                {
                    valueExpr.Type
                });

            return Expression.Call(this.stringBuilderObjExpr, mi, valueExpr);
        }

        /// <summary>
        ///     Формирует дерево выражений для вызова StringBuilder.ToString()
        /// </summary>
        /// <returns></returns>
        private Expression BuildToStringExpression()
        {
            var mi = typeof(StringBuilder).GetMethod(
                nameof(StringBuilder.ToString), new Type[0]);

            return Expression.Call(this.stringBuilderObjExpr, mi);
        }
    }
}