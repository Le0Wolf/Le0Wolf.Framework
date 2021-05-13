// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Common.Attributes
{
    #region usings

    using System;

    #endregion

    /// <summary>
    ///     Атрибут конфигурации, загружаемой при старте приложения
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StartupConfigAttribute : Attribute
    {
        /// <summary>
        ///     Создает экземпляр <see cref="StartupConfigAttribute" />
        /// </summary>
        /// <param name="sectionName">Имя секции конфига</param>
        public StartupConfigAttribute(string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            this.SectionName = sectionName;
        }

        public string SectionName { get; }
    }
}