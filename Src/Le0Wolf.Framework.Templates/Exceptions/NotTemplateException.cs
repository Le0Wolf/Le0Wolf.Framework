// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Exceptions
{
    #region usings

    using System;

    #endregion

    public class NotTemplateException : TemplateEngineException
    {
        public NotTemplateException(Type type) : base(
            $"{type.FullName} is not recognized as template type") { }
    }
}