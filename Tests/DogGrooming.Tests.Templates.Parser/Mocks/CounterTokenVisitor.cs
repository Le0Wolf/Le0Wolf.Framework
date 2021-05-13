// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace DogGrooming.Tests.Templates.Parser.Mocks
{
    #region usings

    #endregion

    internal class CounterTokenVisitor : ITokensVisitor
    {
        public int ActualLiteralCount { get; private set; }

        public int ActualOpenBrackedCount { get; private set; }

        public int ActualCloseBrackedCount { get; private set; }

        public void VisitLiteralToken(Position position, string literalValue)
        {
            this.ActualLiteralCount++;
        }

        public void VisitOpenBrackedToken(Position position)
        {
            this.ActualOpenBrackedCount++;
        }

        public void VisitCloseBrackedToken(Position position)
        {
            this.ActualCloseBrackedCount++;
        }

        public void OnEnd() { }
    }
}