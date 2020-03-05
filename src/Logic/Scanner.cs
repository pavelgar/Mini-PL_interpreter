using System.Collections.Generic;

namespace miniPL {
    public class Scanner {
        private readonly string source;
        private int start = 0;
        private int current = 0;
        private int line = 1;
        private int column = 1;

        public Scanner(string source) {
            this.source = source;
        }

        public List<Token> ScanTokens() {
            List<Token> tokens = new List<Token>();
            while (!IsEOF()) tokens.Add(ScanToken());
            return tokens;
        }

        private Token ScanToken() {
            start = current;

            if (IsEOF()) return new Token(TokenType.EOF, "", null, line);

            char c = Advance();

            switch (c) {
                // Ignore whitespace and count newlines
                // Return the next valid token (recursive)
                case ' ':
                    return ScanToken();
                case '\r':
                    return ScanToken();
                case '\t':
                    return ScanToken();
                case '\n':
                    return ScanToken();

                    // Single character tokens
                case '+':
                    return CreateToken(TokenType.ADD, null);
                case '-':
                    return CreateToken(TokenType.SUB, null);
                case '*':
                    return CreateToken(TokenType.MULT, null);
                case '<':
                    return CreateToken(TokenType.LT, null);
                case '=':
                    return CreateToken(TokenType.EQ, null);
                case '&':
                    return CreateToken(TokenType.AND, null);
                case '!':
                    return CreateToken(TokenType.NOT, null);
                case ';':
                    return CreateToken(TokenType.SEMICOLON, null);
                case '(':
                    return CreateToken(TokenType.LEFT_PAREN, null);
                case ')':
                    return CreateToken(TokenType.RIGHT_PAREN, null);

                    // Multi-character tokens
                case '.':
                    if (Match('.')) return CreateToken(TokenType.RANGE, null);
                    Program.Error(line, $"Unexpected character '{c}'.");
                    return CreateToken(TokenType.SCAN_ERROR, null);
                case ':':
                    return CreateToken(
                        (Match('=') ? TokenType.ASSIGN : TokenType.COLON), null
                    );

                    // Division or comment
                case '/':
                    if (Match('/')) {
                        // It's a single-line comment. Consume it 'till the end of line
                        // and return next valid token
                        while (Peek() != '\n' && !IsEOF()) Advance();
                        return ScanToken();

                    } else if (Match('*')) {
                        // It's a multiline comment. Consume it and return next valid token.
                        ParseMultilineComment();
                        return ScanToken();

                    }
                    return CreateToken(TokenType.DIV, null);

                    // String literal
                case '"':
                    string s = ParseString();
                    // Return SCAN_ERROR if string is unterminated
                    if (s == null) {
                        Program.Error(line, $"Unterminated string.");
                        return CreateToken(TokenType.SCAN_ERROR, null);
                    }

                    return CreateToken(TokenType.STRING, s);

                    // Number literal
                case var digit when char.IsDigit(c):
                    return CreateToken(TokenType.INTEGER, ParseNumber());

                // Identifiers and keywords
                case var letter when char.IsLetter(c): {
                    (TokenType type, object literal) = ParseIdentifier();
                    return CreateToken(type, literal);
                }

                default:
                    Program.Error(line, $"Unexpected character '{c}'.");
                    return CreateToken(TokenType.SCAN_ERROR, null);
            }
        }

        private(TokenType type, object literal) ParseIdentifier() {
            // Read while there are letters, digits or underscores
            while (char.IsLetterOrDigit(Peek()) || Peek() == '_') Advance();

            string text = source.Substring(start, current - start);

            // Check if the identifier is a reserved keyword
            switch (text) {
                case "var":
                    return (TokenType.VAR, null);
                case "for":
                    return (TokenType.FOR, null);
                case "end":
                    return (TokenType.END, null);
                case "in":
                    return (TokenType.IN, null);
                case "do":
                    return (TokenType.DO, null);
                case "read":
                    return (TokenType.READ, null);
                case "print":
                    return (TokenType.PRINT, null);
                case "int":
                    return (TokenType.INT, null);
                case "string":
                    return (TokenType.STR, null);
                case "bool":
                    return (TokenType.BOOL, null);
                case "true":
                    return (TokenType.BOOLEAN, true);
                case "false":
                    return (TokenType.BOOLEAN, false);
                case "assert":
                    return (TokenType.ASSERT, null);
                default:
                    return (TokenType.IDENT, text);
            }
        }

        private double ParseNumber() {
            // Read while there are digits
            while (char.IsDigit(Peek())) Advance();
            string literal = source.Substring(start, current - start);
            return double.Parse(literal);
        }

        private string ParseString() {
            // Remember the start of the string. Used for error handling.
            var stringStart = line;
            // Read characters until EOF or closing ".
            while (!IsEOF() && Peek() != '"') {
                // Skip twice if next character is escaped
                if (Peek() == '\\' && PeekNext() != '\n') {
                    Advance();
                }
                Advance();
            }

            // Catch unterminated string
            if (IsEOF()) {
                Program.Error(stringStart, "Unterminated string.");
                return null;
            }

            // Consume the closing ".
            Advance();

            // Trim the surrounding quotes
            return source.Substring(start + 1, current - start - 2);
        }

        private void ParseMultilineComment() {
            // Remember the start of the comment. Used for error handling.
            var commentStart = line;
            // Read characters until EOF or closing */.
            while (!IsEOF() && (Peek() != '*' || PeekNext() != '/')) {
                // Check if a new multiline comment is starting /*
                if (Peek() == '/' && PeekNext() == '*') {
                    Advance();
                    Advance();
                    ParseMultilineComment();
                } else {
                    Advance();
                }
            }

            // Catch unterminated comment
            if (IsEOF()) {
                Program.Error(commentStart, "Unterminated multiline comment.");
                return;
            }

            // Consume the closing */
            Advance();
            Advance();
        }

        private char Advance() {
            if (source[current] == '\n') {
                line++;
                column = 1;
            } else {
                column++;
            }
            return source[current++];
        }

        private char Peek() {
            // Check for end of source
            if (IsEOF()) return '\0';
            return source[current];
        }

        private char PeekNext() {
            // Check for end of source
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }

        private bool Match(char expected) {
            // Check for end of source
            if (IsEOF()) return false;
            // Check if the next character matches expected
            if (source[current] != expected) return false;

            Advance();
            return true;
        }

        private bool IsEOF() {
            return current >= source.Length;
        }

        private Token CreateToken(TokenType type, object literal) {
            string raw = source.Substring(start, current - start);
            return new Token(type, raw, literal, line);
        }
    }
}
