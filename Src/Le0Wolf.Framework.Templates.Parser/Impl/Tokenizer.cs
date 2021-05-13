// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Parser.Impl
{
    #region usings

    using System;
    using System.Text;

    #endregion

    /// <summary>
    ///     Класс, занимающийся разбивкой текста на токены
    /// </summary>
    public class Tokenizer : ITemplateTokenizer
    {
        private enum State
        {
            BeginReadLiteral,
            ReadLiteral,
            ReadOpenBracked,
            ReadCloseBracked
        }

        private enum LexemType
        {
            Char,
            OpenBracked,
            CloseBracked
        }

        /// <summary>
        ///     Выполняет обработку переданного текста, разбивая их на токены и передавая в визитор
        /// </summary>
        /// <param name="templateText"></param>
        /// <param name="visitor"></param>
        public void Tokenize(string templateText, ITokensVisitor visitor)
        {
            if (string.IsNullOrEmpty(templateText))
            {
                throw new ArgumentNullException(nameof(templateText));
            }

            var currentLineAndCol = new Position(1, 0);
            var tokenStartLineAndCol = new Position();
            var value = new StringBuilder();
            var state = State.ReadLiteral;

            void UpdateTokenStartLineAndCol()
            {
                tokenStartLineAndCol.Col = currentLineAndCol.Col;
                tokenStartLineAndCol.Row = currentLineAndCol.Row;
            }

            for (var i = 0; i < templateText.Length; i++)
            {
                var chr = templateText[i];
                switch (state)
                {
                    case State.ReadLiteral:
                        var lexemType = this.GetLexemType(templateText, i);
                        if (lexemType == LexemType.Char)
                        {
                            value.Append(chr);
                        }
                        else
                        {
                            visitor.VisitLiteralToken(tokenStartLineAndCol,
                                value.ToString());
                            value.Clear();
                            UpdateTokenStartLineAndCol();

                            state = lexemType == LexemType.OpenBracked
                                ? State.ReadOpenBracked
                                : State.ReadCloseBracked;
                        }

                        break;
                    case State.ReadOpenBracked:
                        visitor.VisitOpenBrackedToken(tokenStartLineAndCol);
                        state = State.BeginReadLiteral;

                        break;
                    case State.ReadCloseBracked:
                        visitor.VisitCloseBrackedToken(tokenStartLineAndCol);
                        state = State.BeginReadLiteral;

                        break;
                    case State.BeginReadLiteral:
                        UpdateTokenStartLineAndCol();
                        state = State.ReadLiteral;
                        goto case State.ReadLiteral;
                }

                this.MovePositionByChar(currentLineAndCol, chr);
            }

            if (state == State.ReadLiteral && value.Length > 0)
            {
                visitor.VisitLiteralToken(tokenStartLineAndCol,
                    value.ToString());
            }

            visitor.OnEnd();
        }

        private void MovePositionByChar(Position position, char chr)
        {
            if (chr == '\n')
            {
                position.AppendRow();
            }
            else
            {
                position.AppendCol();
            }
        }

        private static bool IsLastPosition(int pos, int length)
        {
            return pos == length - 1;
        }

        private LexemType GetLexemType(string text, int pos)
        {
            if (!IsLastPosition(pos, text.Length))
            {
                var chr = text[pos];
                var nextChr = text[pos + 1];
                switch (chr)
                {
                    case '{' when nextChr == '{':
                        return LexemType.OpenBracked;
                    case '}' when nextChr == '}':
                        return LexemType.CloseBracked;
                }
            }

            return LexemType.Char;
        }
    }
}