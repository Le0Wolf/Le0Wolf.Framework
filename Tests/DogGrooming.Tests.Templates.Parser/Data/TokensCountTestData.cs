// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace DogGrooming.Tests.Templates.Parser.Data
{
    public class TokensCountTestData : BaseTokensTestData
    {
        public TokensCountTestData(string text, int expectedLiteralCount,
            int expectedOpenBrackedCount, int expectedCloseBrackedCount)
            : base(text)
        {
            this.ExpectedLiteralCount = expectedLiteralCount;
            this.ExpectedOpenBrackedCount = expectedOpenBrackedCount;
            this.ExpectedCloseBrackedCount = expectedCloseBrackedCount;
        }

        public int ExpectedLiteralCount { get; set; }

        public int ExpectedOpenBrackedCount { get; set; }

        public int ExpectedCloseBrackedCount { get; set; }
    }
}