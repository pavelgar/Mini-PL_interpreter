namespace miniPL {
    public struct Token {
        public readonly TokenType type;
        public readonly string rawValue;
        public readonly object literal;
        public readonly int line;
        public Token(TokenType type, string rawValue, object literal, int line) {
            this.type = type;
            this.rawValue = rawValue;
            this.literal = literal;
            this.line = line;
        }

        public override string ToString() {
            return "Token(line: " + line +
                ", type: " + type +
                ", raw: " + rawValue +
                ", literal: " + literal +
                ")";
        }
    }
}
