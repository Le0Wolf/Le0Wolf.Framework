// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Parser
{
    /// <summary>
    ///     Визитор, который обрабатывает токены, продуцируемые токенизатором
    /// </summary>
    public interface ITokensVisitor
    {
        /// <summary>
        ///     Выполняет отбработку открывающей скобки
        /// </summary>
        /// <param name="position">Позиция токена в тексте</param>
        void VisitOpenBrackedToken(Position position);

        /// <summary>
        ///     Выполняет отбработку закрывающей скобки
        /// </summary>
        /// <param name="position">Позиция токена в тексте</param>
        void VisitCloseBrackedToken(Position position);

        /// <summary>
        ///     Выполняет обработку строкового литерала
        /// </summary>
        /// <param name="position">Позиция токена в тексте</param>
        /// <param name="literalValue">Значеие литерала</param>
        void VisitLiteralToken(Position position, string literalValue);

        /// <summary>
        ///     Вызывается в конце  цикла обработкии
        /// </summary>
        void OnEnd();
    }
}