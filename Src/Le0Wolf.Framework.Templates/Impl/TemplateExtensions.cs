// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Impl
{
    #region usings

    using System;
    using System.Reflection;

    using Attributes;

    using Exceptions;

    #endregion

    internal static class TemplateExtensions
    {
        public static bool IsValidTemplate<TTemplate>()
        {
            return Attribute.IsDefined(typeof(TTemplate),
                typeof(TemplateAttribute));
        }

        public static (string name, string description) GetTemplateInfo<
            TTemplate>()
        {
            return GetTemplateInfo(typeof(TTemplate));
        }

        public static (string name, string description) GetTemplateInfo(
            Type templateType)
        {
            var attr = templateType.GetCustomAttribute<TemplateAttribute>();

            if (attr == null)
            {
                throw new NotTemplateException(templateType);
            }

            var name = attr.Name ?? templateType.FullName;

            return (name, attr.Desccription);
        }

        public static string GetTemplateName(Type templateType)
        {
            var attr = templateType.GetCustomAttribute<TemplateAttribute>();

            if (attr == null)
            {
                throw new NotTemplateException(templateType);
            }

            return attr.Name ?? templateType.FullName;
        }

        public static string GetTemplateName<TTemplate>()
        {
            return GetTemplateName(typeof(TTemplate));
        }
    }
}