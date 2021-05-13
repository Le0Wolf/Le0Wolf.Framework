// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace DogGrooming.Tests.Templates.Parser.Data
{
    public abstract class BaseTokensTestData
    {
        protected BaseTokensTestData(string text)
        {
            this.Text = text;
        }

        public string Text { get; set; }
    }
}