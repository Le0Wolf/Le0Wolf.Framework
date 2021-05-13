// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C

namespace DogGrooming.Tests.Templates.Parser.Data
{
    #region usings

    using System;
    using System.Collections.Generic;

    #endregion

    public class Token : IEquatable<Token>
    {
        public Token(Position position, string value)
        {
            this.Position = position ??
                            throw new ArgumentNullException(nameof(position));
            this.Value
                = value ?? throw new ArgumentNullException(nameof(value));
            this.TokenType = TokenType.Literal;
        }

        public Token(Position position, TokenType tokenType)
        {
            this.Position = position ??
                            throw new ArgumentNullException(nameof(position));
            this.TokenType = tokenType;
        }

        public Position Position { get; set; }

        public string Value { get; set; }

        public TokenType TokenType { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Token);
        }

        public bool Equals(Token other)
        {
            return other != null &&
                   EqualityComparer<Position>.Default.Equals(this.Position,
                       other.Position) && this.Value == other.Value &&
                   this.TokenType == other.TokenType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Position, this.Value, this.TokenType);
        }

        public static bool operator ==(Token left, Token right)
        {
            return EqualityComparer<Token>.Default.Equals(left, right);
        }

        public static bool operator !=(Token left, Token right)
        {
            return !(left == right);
        }
    }
}