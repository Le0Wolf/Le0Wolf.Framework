// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Extensions
{
    #region usings

    using Impl;

    using Microsoft.Extensions.DependencyInjection;

    #endregion

    public static class TemplateRegistrationExtensions
    {
        public static void RegisterTemplateType<TTemplate>(
            this IServiceCollection services)
        {
            services
                .AddScoped<ICompiledTemplate<TTemplate>,
                    CompiledTemplate<TTemplate>>();
        }
    }
}