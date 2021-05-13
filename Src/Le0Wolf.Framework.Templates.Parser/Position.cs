// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace Le0Wolf.Framework.Templates.Parser
{
    #region usings

    using System.Diagnostics;

    #endregion

    /// <summary>
    ///     Позиция в тексте
    /// </summary>
    public record Position
    {
        public Position(Position position)
        {
            this.Row = position.Row;
            this.Col = position.Col;
        }

        public Position()
        {
            this.Row = 1;
            this.Col = 1;
        }

        public Position(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        /// <summary>
        ///     Номер строки
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        ///     Порядковый номер символа в строке
        /// </summary>
        public int Col { get; set; }

        [DebuggerHidden]
        public void AppendCol()
        {
            this.Col++;
        }

        [DebuggerHidden]
        public void AppendRow()
        {
            this.Col = 1;
            this.Row++;
        }

        public override string ToString()
        {
            return $"Row = {this.Row}, Col = {this.Col}";
        }
    }
}