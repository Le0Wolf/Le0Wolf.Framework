// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Impl
{
    #region usings

    using System;
    using System.Collections.Concurrent;

    using Infrastructure;

    #endregion

    public class
        CompiledTemplate<TTemplateArgs> : ICompiledTemplate<TTemplateArgs>
    {
        private readonly ITemplatesStore templatesStore;
        private readonly ITemplateCompiler templateCompiler;

        private static readonly
            ConcurrentDictionary<string, Func<TTemplateArgs, string>>
            CompiledTemplates = new();

        private static readonly object s_lock = new();

        public CompiledTemplate(ITemplatesStore templatesStore,
            ITemplateCompiler templateCompiler)
        {
            this.templatesStore = templatesStore;
            this.templateCompiler = templateCompiler;
        }

        public string Apply(TTemplateArgs templateArgs, string variant = null)
        {
            if (string.IsNullOrEmpty(variant))
            {
                variant = "default";
            }

            var template = CompiledTemplates.GetOrAdd(
                variant,
                key =>
                {
                    lock (s_lock)
                    {
                        var templateText = this.templatesStore
                            .GetTemplateText<TTemplateArgs>(key);

                        return this.templateCompiler.Compile<TTemplateArgs>(
                            templateText);
                    }
                });

            return template(templateArgs);
        }

        public void Update(string templateText, string variant = null)
        {
            if (string.IsNullOrEmpty(variant))
            {
                variant = "default";
            }

            Func<TTemplateArgs, string> Fn(string key)
            {
                var template
                    = this.templateCompiler.Compile<TTemplateArgs>(
                        templateText);
                this.templatesStore.UpdateTemplateText<TTemplateArgs>(
                    templateText, variant);

                return template;
            }

            CompiledTemplates.AddOrUpdate(variant, Fn, (key, old) => Fn(key));
        }
    }
}