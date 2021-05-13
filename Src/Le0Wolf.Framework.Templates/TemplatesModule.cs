// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates
{
    #region usings

    using Impl;

    using Infrastructure;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    using IStartup = Modules.IStartup;

    #endregion

    public class TemplatesModule : IStartup
    {
        #region Implementation of IStartup

        /// <summary>
        ///     Место для регистрации сервисов
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITemplateCompiler, TemplateCompiller>();

            //services.AddScoped(typeof(ICompiledTemplate<>),
            //    typeof(CompiledTemplate<>));
        }

        /// <summary>
        ///     Место для настройки приложения
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env) { }

        #endregion
    }
}