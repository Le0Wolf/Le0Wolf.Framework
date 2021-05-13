// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Infrastructure
{
    #region usings

    using System;

    #endregion

    /// <summary>
    ///     Интерфейс компилятора шаблонов
    /// </summary>
    public interface ITemplateCompiler
    {
        /// <summary>
        ///     Компилирует шаблон в делегат
        /// </summary>
        /// <typeparam name="TTemplateArgs">Тип аргументов шаблона</typeparam>
        /// <param name="templateText">Текст шаблона</param>
        /// <returns>Делегат, заполняющий переменные шаблона из свойств переданного объекта</returns>
        Func<TTemplateArgs, string> Compile<TTemplateArgs>(string templateText);
    }
}