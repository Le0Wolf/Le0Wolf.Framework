// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Parser.Exceptions
{
    #region usings

    using System;

    using Common.Exceptions;

    #endregion

    public class TemplateParserException : AppFrameworkException
    {
        public TemplateParserException(string message) : base(message) { }

        public TemplateParserException(string message, Exception inner) : base(
            message, inner) { }

        public TemplateParserException(Position position, string message) :
            base(message)
        {
            this.Position = position;
        }

        public TemplateParserException(Position position, string message,
            Exception inner) : base(message, inner)
        {
            this.Position = position;
        }

        public Position Position { get; set; }

        public int Code { get; set; }
    }
}