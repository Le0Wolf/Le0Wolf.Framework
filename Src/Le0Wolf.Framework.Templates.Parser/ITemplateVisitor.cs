// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Parser
{
    /// <summary>
    ///     Визитор, который обрабатывает элемены шаблона
    /// </summary>
    public interface ITemplateVisitor
    {
        /// <summary>
        ///     Обрабатывает простую строку
        /// </summary>
        /// <param name="value">Строка</param>
        void VisitLiteral(string value);

        /// <summary>
        ///     Обрабаттывает имя переменной
        /// </summary>
        /// <param name="name">Имя переменной</param>
        void VisitVariable(string name);
    }
}