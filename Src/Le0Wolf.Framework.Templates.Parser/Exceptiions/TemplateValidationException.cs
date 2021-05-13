// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Parser.Exceptions
{
    #region usings

    using System;

    #endregion

    public class TemplateValidationException : TemplateParserException
    {
        public TemplateValidationException(Position position, string message) :
            base(position, message) { }

        public TemplateValidationException(Position position, string message,
            Exception inner) : base(position, message, inner) { }

        public TemplateValidationException(Position position, string message,
            string value) : base(position, message)
        {
            this.Value = value;
        }

        public TemplateValidationException(Position position, string message,
            string value, Exception inner) : base(position, message, inner)
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }
}