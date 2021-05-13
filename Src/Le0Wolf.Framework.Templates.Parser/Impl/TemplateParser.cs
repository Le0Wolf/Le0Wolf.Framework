// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Parser.Impl
{
    public class TemplateParser : ITemplateParser
    {
        private readonly ITemplateTokenizer tokenizer;

        public TemplateParser(ITemplateTokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        public void Parse(string templateText, ITemplateVisitor visitor)
        {
            var tokensVisitor = new TokensVisitor(visitor);
            this.tokenizer.Tokenize(templateText, tokensVisitor);
        }
    }
}