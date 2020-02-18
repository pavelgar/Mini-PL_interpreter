using System;

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

        public Token ScanToken() {
            // Check for End of File
            if (IsEOF()) return new Token(TokenType.EOF, "", null, line);
            // Set the start of current token to current pointer
            start = current;
            // Read the next character
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
                    Program.Error(line, column, "Unexpected character \"" + c + "\".");
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
                    if (s == null) return CreateToken(TokenType.SCAN_ERROR, null);
                    return CreateToken(TokenType.STRING, s);

                    // Number literal
                case var digit when char.IsDigit(c):
                    return CreateToken(TokenType.INTEGER, ParseNumber());

                // Identifiers and keywords
                case var letter when char.IsLetter(c):
                    return CreateToken(ParseIdentifier(), null);

                default:
                    Program.Error(line, column, "Unexpected character \"" + c + "\".");
                    return CreateToken(TokenType.SCAN_ERROR, null);
            }
        }

        private TokenType ParseIdentifier() {
            // Read while there are letters, digits or underscores
            while (char.IsLetterOrDigit(Peek()) || Peek() == '_') Advance();

            string text = source.Substring(start, current - start);

            // Check if the identifier is a reserved keyword
            switch (text) {
                case "var":
                    return TokenType.VAR;
                case "for":
                    return TokenType.FOR;
                case "end":
                    return TokenType.END;
                case "in":
                    return TokenType.IN;
                case "do":
                    return TokenType.DO;
                case "read":
                    return TokenType.READ;
                case "print":
                    return TokenType.PRINT;
                case "int":
                    return TokenType.INT;
                case "string":
                    return TokenType.STR;
                case "bool":
                    return TokenType.BOOL;
                case "assert":
                    return TokenType.ASSERT;
                default:
                    return TokenType.IDENT;
            }
        }

        private int ParseNumber() {
            // Read while there are digits
            while (char.IsDigit(Peek())) Advance();
            string literal = source.Substring(start, current - start);
            return int.Parse(literal);
        }

        private string ParseString() {
            // Remember the start of the string. Used for error handling.
            var stringStart = (line, column);
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
                Program.Error(
                    stringStart.line,
                    stringStart.column,
                    "Unterminated string."
                );
                return null;
            }

            // Consume the closing ".
            Advance();

            // Trim the surrounding quotes
            return source.Substring(start + 1, current - start - 2);
        }

        private void ParseMultilineComment() {
            // Remember the start of the comment. Used for error handling.
            var stringStart = (line, column);
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
                Program.Error(
                    stringStart.line,
                    stringStart.column,
                    "Unterminated multiline comment."
                );
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
