// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates
{
    /// <summary>
    ///     Хранит в себе скомпилированный шаблон
    /// </summary>
    public interface ICompiledTemplate<TTemplateArgs>
    {
        /// <summary>
        ///     Подставляет переданные аргументы в шаблон
        /// </summary>
        /// <param name="args">Аргументы для подстановки в шаблон</param>
        /// <param name="variant">Вариант шаблона</param>
        /// <returns>Текст, в котором все переменные шаблона заменены на значения из переданных аргументов</returns>
        string Apply(TTemplateArgs args, string variant = null);

        /// <summary>
        ///     Обновляет скомпилированное выражение и сохраняет текст в хранилище
        /// </summary>
        /// <param name="templateText">Новый текст шаблона</param>
        /// <param name="variant">Вариант шаблона</param>
        void Update(string templateText, string variant = null);
    }
}