// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Infrastructure
{
    #region usings

    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    /// <summary>
    ///     Хранилище текстов шаблонов
    /// </summary>
    public interface ITemplatesStore
    {
        /// <summary>
        ///     Получает описание шаблона
        /// </summary>
        /// <typeparam name="TTemplate"></typeparam>
        /// <returns></returns>
        string GetTemplateDescription<TTemplate>();

        /// <summary>
        ///     Получает текст шаблона
        /// </summary>
        /// <typeparam name="TTemplate"></typeparam>
        /// <param name="variant">Вариант шаблона</param>
        /// <returns></returns>
        string GetTemplateText<TTemplate>(string variant = null);

        /// <summary>
        ///     Получает текст шаблона
        /// </summary>
        /// <typeparam name="TTemplate"></typeparam>
        /// <param name="variant">Вариант шаблона</param>
        /// <returns></returns>
        Task<string> GetTemplateTextAsync<TTemplate>(string variant = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Обновляет текст шаблона
        /// </summary>
        /// <typeparam name="TTemplate">Тип шаблона</typeparam>
        /// <param name="newTemplateText">Новый текст шаблона</param>
        /// <param name="variant">Вариант шаблона</param>
        void UpdateTemplateText<TTemplate>(string newTemplateText,
            string variant = null);

        /// <summary>
        ///     Обновляет текст шаблона
        /// </summary>
        /// <typeparam name="TTemplate">Тип шаблона</typeparam>
        /// <param name="newTemplateText">Новый текст шаблона</param>
        /// <param name="variant">Вариант шаблона</param>
        Task UpdateTemplateTextAsync<TTemplate>(string newTemplateText,
            string variant = null,
            CancellationToken cancellationToken = default);
    }
}