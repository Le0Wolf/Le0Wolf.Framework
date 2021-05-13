// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace DogGrooming.Tests.Templates.Parser.Mocks
{
    #region usings

    using System;

    #endregion

    public class TestTemplateVisitor : ITemplateVisitor
    {
        public event Action<string> OnVisitLiteral;

        public event Action<string> OnVisitVariable;

        public void VisitLiteral(string value)
        {
            this.OnVisitLiteral?.Invoke(value);
        }

        public void VisitVariable(string name)
        {
            this.OnVisitVariable?.Invoke(name);
        }
    }
}