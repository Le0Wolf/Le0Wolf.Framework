// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Parser
{
    /// <summary>
    ///     Интерфейс парсера шаблонов
    /// </summary>
    public interface ITemplateParser
    {
        /// <summary>
        ///     Выполняет парсинг шаблона
        /// </summary>
        /// <param name="templateText">Текст шаблона</param>
        /// <param name="visitor">Обрабоотчик элементов шаблона</param>
        void Parse(string templateText, ITemplateVisitor visitor);
    }
}