// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Modules
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Attributes;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    [NonModule]
    public class ModulesLoader : IStartup
    {
        private readonly List<IStartup> modules;

        public ModulesLoader(params object[] additionalDependencies)
        {
            this.modules = new List<IStartup>();

            var all = Assembly
                .GetEntryAssembly()
                .GetReferencedAssemblies()
                .Select(Assembly.Load)
                .SelectMany(x => x.DefinedTypes)
                .Where(type =>
                    typeof(IStartup).IsAssignableFrom(type) && type.IsPublic &&
                    !type.IsGenericType && !type.IsAbstract)
                .Where(type =>
                    !Attribute.IsDefined(type, typeof(NonModuleAttribute)))
                .ToList();

            var dependenciesTypes
                = additionalDependencies.ToDictionary(x => x.GetType());

            foreach (var moduleType in all)
            {
                var constructors
                    = moduleType.GetConstructors(BindingFlags.Public |
                                                 BindingFlags.Instance);

                ConstructorInfo bestMatch = null;
                ParameterInfo[] bestMatchParams = null;
                var bestMatchParamsCount = -1;
                foreach (var constructor in constructors)
                {
                    var constructorParams = constructor.GetParameters();
                    var matchParamsCount = -1;
                    foreach (var parameter in constructorParams)
                    {
                        if (dependenciesTypes.ContainsKey(parameter
                                .ParameterType)
                            || dependenciesTypes.Keys.Any(x =>
                                parameter.ParameterType.IsAssignableFrom(x)))
                        {
                            matchParamsCount++;
                        }
                        else
                        {
                            if (!parameter.IsOptional)
                            {
                                // Dependency for required parameter is not found
                                matchParamsCount = -1;
                            }

                            break;
                        }
                    }

                    if (bestMatchParamsCount < matchParamsCount ||
                        constructorParams.Length == 0)
                    {
                        bestMatch = constructor;
                        bestMatchParamsCount = constructorParams.Length == 0
                            ? 0
                            : matchParamsCount;
                        bestMatchParams = constructorParams;
                    }
                }

                if (bestMatchParamsCount < 0)
                {
                    throw new InvalidOperationException(
                        $"Unable to load module type '{moduleType.FullName}'.");
                }

                var values = bestMatchParams.Select(x =>
                        dependenciesTypes.First(dt =>
                                x.ParameterType.IsAssignableFrom(dt.Key))
                            .Value)
                    .ToArray();
                var startup = (IStartup)bestMatch.Invoke(values);
                this.modules.Add(startup);
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            foreach (var module in this.modules)
            {
                module.ConfigureServices(services);
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            foreach (var module in this.modules)
            {
                module.Configure(app, env);
            }
        }
    }
}