// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Parser
{
    /// <summary>
    ///     Интерфейс токенизатора шаблона
    /// </summary>
    public interface ITemplateTokenizer
    {
        /// <summary>
        ///     Выполняет токенизацию шаблона
        /// </summary>
        /// <param name="templateText">Текст шаблона</param>
        /// <param name="visitor">Обработчик токенов</param>
        void Tokenize(string templateText, ITokensVisitor visitor);
    }
}