// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace DogGrooming.Tests.Templates.Parser.Data
{
    public class TokensOrderTestData : BaseTokensTestData
    {
        public TokensOrderTestData(string text, params Token[] tokens) :
            base(text)
        {
            this.Tokens = tokens;
        }

        public Token[] Tokens { get; }
    }
}