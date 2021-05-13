// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Exceptions
{
    #region usings

    using System;

    using Common.Exceptions;

    #endregion

    public class TemplateEngineException : AppFrameworkException
    {
        public TemplateEngineException() { }

        public TemplateEngineException(string message) : base(message) { }

        public TemplateEngineException(string message, Exception inner) : base(
            message, inner) { }
    }
}