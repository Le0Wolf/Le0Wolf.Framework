// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace DogGrooming.Tests.Templates.Parser.Mocks
{
    #region usings

    using System.Collections.Generic;

    using Data;

    #endregion

    public class OrderTokenVisitor : ITokensVisitor
    {
        private readonly List<Token> actualTokenTypes = new();

        public IReadOnlyList<Token> ActualTokenTypes =>
            this.actualTokenTypes.AsReadOnly();

        public void VisitLiteralToken(Position position, string literalValue)
        {
            this.actualTokenTypes.Add(new Token(new Position(position),
                literalValue));
        }

        public void VisitOpenBrackedToken(Position position)
        {
            this.actualTokenTypes.Add(new Token(new Position(position),
                TokenType.OpenBracked));
        }

        public void VisitCloseBrackedToken(Position position)
        {
            this.actualTokenTypes.Add(new Token(new Position(position),
                TokenType.CloseBracked));
        }

        public void OnEnd() { }
    }
}