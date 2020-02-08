namespace miniPL {
    public struct Token {
        readonly TokenType type;
        readonly string rawValue;
        readonly object literal;
        readonly int line;
        public Token(TokenType type, string rawValue, object literal, int line) {
            this.type = type;
            this.rawValue = rawValue;
            this.literal = literal;
            this.line = line;
        }

        public override string ToString() {
            return "Token(" + type + ", " + rawValue + ")";
        }
    }
}
