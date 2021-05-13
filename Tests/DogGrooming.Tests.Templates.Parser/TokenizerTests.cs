// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace DogGrooming.Tests.Templates.Parser
{
    #region usings

    using System.Collections.Generic;
    using System.Linq;

    using Data;

    using FluentAssertions;

    using Mocks;

    using Xunit;

    #endregion

    public class TokenizerTests
    {
        [Theory(DisplayName = "Количество токенов")]
        [MemberData(nameof(GetTokensCountTestData))]
        public void TokensCount(TokensCountTestData testdata)
        {
            var mock = new CounterTokenVisitor();
            var tokenizer = new Tokenizer();

            tokenizer.Tokenize(testdata.Text, mock);

            mock.ActualLiteralCount.Should().Be(testdata.ExpectedLiteralCount);
            mock.ActualOpenBrackedCount.Should()
                .Be(testdata.ExpectedOpenBrackedCount);
            mock.ActualCloseBrackedCount.Should()
                .Be(testdata.ExpectedCloseBrackedCount);
        }

        [Theory(DisplayName = "Порядок токенов")]
        [MemberData(nameof(GetTokensOrderTestData))]
        public void TokensOrder(TokensOrderTestData testdata)
        {
            var mock = new OrderTokenVisitor();
            var tokenizer = new Tokenizer();

            tokenizer.Tokenize(testdata.Text, mock);

            mock.ActualTokenTypes.Select(x => x.TokenType)
                .Should()
                .Equal(testdata.Tokens.Select(x => x.TokenType));
            mock.ActualTokenTypes.Select(x => x.Value)
                .Should()
                .Equal(testdata.Tokens.Select(x => x.Value));
            mock.ActualTokenTypes.Select(x => x.Position)
                .Should()
                .Equal(testdata.Tokens.Select(x => x.Position));
            mock.ActualTokenTypes.Should().Equal(testdata.Tokens);
        }

        public static IEnumerable<object[]> GetTokensCountTestData()
        {
            var text = @"
q { w {{a t}}i{{k{{z}
lj}}
ld{{sah";

            return new List<object[]>
            {
                new object[]
                {
                    new TokensCountTestData(text, 7, 4, 2)
                }
            };
        }

        public static IEnumerable<object[]> GetTokensOrderTestData()
        {
            var text = @"
q { w {{a t}}i{{k{{z}
lj}}
ld{{sah";

            return new List<object[]>
            {
                new object[]
                {
                    new TokensOrderTestData(
                        text,
                        new Token(new Position(1, 1), "\r\nq { w "),
                        new Token(new Position(2, 7), TokenType.OpenBracked),
                        new Token(new Position(2, 9), "a t"),
                        new Token(new Position(2, 12), TokenType.CloseBracked),
                        new Token(new Position(2, 14), "i"),
                        new Token(new Position(2, 15), TokenType.OpenBracked),
                        new Token(new Position(2, 17), "k"),
                        new Token(new Position(2, 18), TokenType.OpenBracked),
                        new Token(new Position(2, 20), "z}\r\nlj"),
                        new Token(new Position(3, 3), TokenType.CloseBracked),
                        new Token(new Position(3, 5), "\r\nld"),
                        new Token(new Position(4, 3), TokenType.OpenBracked),
                        new Token(new Position(4, 5), "sah"))
                }
            };
        }
    }
}