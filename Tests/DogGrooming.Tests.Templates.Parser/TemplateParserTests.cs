// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace DogGrooming.Tests.Templates.Parser
{
    #region usings

    using System;

    using FluentAssertions;

    using Mocks;

    using Xunit;

    #endregion

    /// <summary>
    ///     Интеграционные тесты
    /// </summary>
    public class TemplateParserTests
    {
        private readonly string testTemplate = @"
q { w {{a t}}i{{k{{z}
lj}}
ld{{sah";

        private readonly string testTemplate2 = @"
q { w {{a_t}}i{{k}}z}
lj} }
ldsah";

        [Fact]
        public void TestValidation()
        {
            var mockVisitor = new TestTemplateVisitor();

            var tokenizer = new Tokenizer();
            var templateParser = new TemplateParser(tokenizer);

            Action act = () =>
                templateParser.Parse(this.testTemplate, mockVisitor);

            act.Should().Throw<TemplateParserException>();
        }

        [Fact]
        public void TestTemplateParsing()
        {
            var mockVisitor = new TestTemplateVisitor();
            var countLiterals = 0;
            var countVariables = 0;
            mockVisitor.OnVisitLiteral += x => countLiterals++;
            mockVisitor.OnVisitVariable += x => countVariables++;

            var tokenizer = new Tokenizer();
            var templateParser = new TemplateParser(tokenizer);

            templateParser.Parse(this.testTemplate2, mockVisitor);

            countLiterals.Should().Be(3);
            countVariables.Should().Be(2);
        }
    }
}