// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Modules
{
    #region usings

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    /// <summary>
    ///     Интерфейс инициализатора модуля
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        ///     Место для регистрации сервисов
        /// </summary>
        /// <param name="services"></param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        ///     Место для настройки приложения
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        void Configure(IApplicationBuilder app, IWebHostEnvironment env);
    }
}