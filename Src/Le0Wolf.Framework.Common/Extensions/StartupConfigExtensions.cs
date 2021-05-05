// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Common.Extensions
{
    #region usings

    using System;
    using System.Reflection;

    using Attributes;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    /// <summary>
    ///     Методы расширения для коллекции сервисов
    /// </summary>
    public static class StartupConfigExtensions
    {
        /// <summary>
        ///     Добавляет опции в коллекцию сервисов
        /// </summary>
        /// <typeparam name="TConfig">Тип объекта опций</typeparam>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="configuration">Конфигурация приложения</param>
        /// <returns>Экземпляр конфигурации</returns>
        public static TConfig AddStartupConfig<TConfig>(
            this IServiceCollection services,
            IConfiguration configuration) where TConfig : class
        {
            var type = typeof(TConfig);
            var sectionName
                = type.GetCustomAttribute<StartupConfigAttribute>()
                      ?.SectionName ??
                  throw new ArgumentException(
                      $"{type.FullName} is not valid config type. Allow only with StartupConfigAttribute");

            var options = configuration.GetSection(sectionName).Get<TConfig>();
            services.AddSingleton(options);

            return options;
        }
    }
}