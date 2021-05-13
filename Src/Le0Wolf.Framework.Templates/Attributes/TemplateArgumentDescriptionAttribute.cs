// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Attributes
{
    #region usings

    using System;

    #endregion

    [AttributeUsage(AttributeTargets.Property)]
    public class TemplateArgumentDescriptionAttribute : Attribute
    {
        public string Description { get; }

        public TemplateArgumentDescriptionAttribute(string description)
        {
            this.Description = description;
        }
    }
}