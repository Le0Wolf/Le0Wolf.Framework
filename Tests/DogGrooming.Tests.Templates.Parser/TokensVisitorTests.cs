// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace DogGrooming.Templates.Parser.Tests
{
    #region usings

    using System;

    using FluentAssertions;

    using Xunit;

    #endregion

    public class TokensVisitorTests
    {
        [Fact]
        public void TestEmpty()
        {
            var mock = new TestTemplateVisitor();
            var visitor = new TokensVisitor(mock);
            visitor.OnEnd();
        }

        [Fact]
        public void TestNotClosedVariable()
        {
            var mock = new TestTemplateVisitor();
            var mockPosition = new Position();

            var visitor = new TokensVisitor(mock);
            Action act = () =>
            {
                visitor.VisitOpenBrackedToken(mockPosition);
                visitor.VisitLiteralToken(mockPosition, "slml");
                visitor.OnEnd(); // throw this
            };

            act.Should()
                .Throw<TemplateParserException>()
                .Where(e => e.Code == 9);
        }

        [Fact]
        public void TestOpenAndCloseBracked()
        {
            var mock = new TestTemplateVisitor();
            var mockPosition = new Position();

            var visitor = new TokensVisitor(mock);
            Action act = () =>
            {
                visitor.VisitOpenBrackedToken(mockPosition);
                visitor.VisitCloseBrackedToken(mockPosition); // throw this
                visitor.OnEnd();
            };

            act.Should()
                .Throw<TemplateParserException>()
                .Where(e => e.Code == 7);
        }

        [Fact]
        public void TestOpenBrackedAfterVariable()
        {
            var mock = new TestTemplateVisitor();
            var mockPosition = new Position();

            var visitor = new TokensVisitor(mock);
            Action act = () =>
            {
                visitor.VisitOpenBrackedToken(mockPosition);
                visitor.VisitLiteralToken(mockPosition, "slml");
                visitor.VisitOpenBrackedToken(mockPosition); // throw this
                visitor.OnEnd();
            };

            act.Should()
                .Throw<TemplateParserException>()
                .Where(e => e.Code == 5);
        }

        [Fact]
        public void TestSingleCloseBracked()
        {
            var mock = new TestTemplateVisitor();
            var mockPosition = new Position();

            var visitor = new TokensVisitor(mock);
            Action act = () =>
            {
                visitor.VisitCloseBrackedToken(mockPosition); // throw this
                visitor.OnEnd();
            };

            act.Should()
                .Throw<TemplateParserException>()
                .Where(e => e.Code == 6);
        }

        [Fact]
        public void TestSingleLiteral()
        {
            var mock = new TestTemplateVisitor();
            var mockPosition = new Position();

            using var monitoredMock = mock.Monitor();

            var visitor = new TokensVisitor(monitoredMock.Subject);
            visitor.VisitLiteralToken(mockPosition, "dvdsv dsvds \r\ndsvsd");
            visitor.OnEnd();

            _ = monitoredMock.Should().Raise(nameof(mock.OnVisitLiteral));
            monitoredMock.Should().NotRaise(nameof(mock.OnVisitVariable));
        }

        [Fact]
        public void TestSingleOpenBracked()
        {
            var mock = new TestTemplateVisitor();
            var mockPosition = new Position();

            var visitor = new TokensVisitor(mock);
            Action act = () =>
            {
                visitor.VisitOpenBrackedToken(mockPosition);
                visitor.OnEnd(); // throw this
            };

            _ = act.Should()
                .Throw<TemplateParserException>()
                .Where(e => e.Code == 8);
        }

        [Fact]
        public void TestTwoOpenBracked()
        {
            var mock = new TestTemplateVisitor();
            var mockPosition = new Position();

            var visitor = new TokensVisitor(mock);
            Action act = () =>
            {
                visitor.VisitOpenBrackedToken(mockPosition);
                visitor.VisitOpenBrackedToken(mockPosition); // throw this
                visitor.OnEnd();
            };

            _ = act.Should()
                .Throw<TemplateParserException>()
                .Where(e => e.Code == 4);
        }

        [Fact]
        public void TestVariable()
        {
            var mock = new TestTemplateVisitor();
            var mockPosition = new Position();

            using var monitoredMock = mock.Monitor();

            var visitor = new TokensVisitor(monitoredMock.Subject);
            Action act = () =>
            {
                visitor.VisitOpenBrackedToken(mockPosition);
                visitor.VisitLiteralToken(mockPosition, "abc");
                visitor.VisitCloseBrackedToken(mockPosition);
                visitor.OnEnd();
            };

            act.Should().NotThrow();
            monitoredMock.Should().Raise(nameof(mock.OnVisitVariable));
            monitoredMock.Should().NotRaise(nameof(mock.OnVisitLiteral));
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("Abc_q", true)]
        [InlineData("Abc q", false)]
        [InlineData("Abc-q", false)]
        [InlineData("Abc\r\nq", false)]
        public void TestVariableNameFormatValidation(string variableName,
            bool positive)
        {
            var mock = new TestTemplateVisitor();
            var mockPosition = new Position();

            var visitor = new TokensVisitor(mock);
            Action act = () =>
            {
                visitor.VisitOpenBrackedToken(mockPosition);
                visitor.VisitLiteralToken(mockPosition,
                    variableName); // throw this
                visitor.VisitCloseBrackedToken(mockPosition);
                visitor.OnEnd();
            };

            if (positive)
            {
                act.Should().NotThrow<TemplateValidationException>();
            }
            else
            {
                act.Should().Throw<TemplateValidationException>();
            }
        }
    }
}