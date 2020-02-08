using System;
using System.Collections.Generic;

namespace miniPL {
    public class Scanner {
        private readonly string source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source) {
            this.source = source;
        }

        public List<Token> ScanTokens() {
            // Parse tokens until the end of source
            while (!IsEOF()) {
                start = current;
                ScanToken();
            }
            // Add EOF token at the end
            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void ScanToken() {
            char c = Advance();

            switch (c) {
                // Ignore whitespace and count newlines
                case ' ':
                    break;
                case '\r':
                    break;
                case '\t':
                    break;
                case '\n':
                    line++;
                    break;

                    // Single character tokens
                case '+':
                    AddToken(TokenType.ADD);
                    break;
                case '-':
                    AddToken(TokenType.SUB);
                    break;
                case '*':
                    AddToken(TokenType.MULT);
                    break;
                case '<':
                    AddToken(TokenType.LT);
                    break;
                case '=':
                    AddToken(TokenType.EQ);
                    break;
                case '&':
                    AddToken(TokenType.AND);
                    break;
                case '!':
                    AddToken(TokenType.NOT);
                    break;
                case ';':
                    AddToken(TokenType.SEMICOLON);
                    break;
                case '(':
                    AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    AddToken(TokenType.RIGHT_PAREN);
                    break;

                    // Multi-character tokens
                case '.':
                    if (Match('.')) {
                        AddToken(TokenType.RANGE);
                    } else {
                        Program.Error(line, "Unexpected character \"" + c + "\".");
                    }
                    break;
                case ':':
                    AddToken(Match('=') ? TokenType.ASSIGN : TokenType.COLON);
                    break;

                    // Division or comment
                case '/':
                    if (Match('/')) {
                        // It's a single-line comment. Consume it 'till the end of line
                        while (Peek() != '\n' && !IsEOF()) Advance();

                    } else if (Match('*')) {
                        // It's a multiline comment.
                        ParseMultilineComment();

                    } else {
                        // It's division
                        AddToken(TokenType.DIV);
                    }
                    break;

                    // String literal
                case '"':
                    ParseString();
                    break;

                    // Number literal
                case var digit when char.IsDigit(c):
                    ParseNumber();
                break;

                // Identifiers and keywords
                case var letter when char.IsLetter(c):
                    ParseIdentifier();
                break;

                default:
                    Program.Error(line, "Unexpected character \"" + c + "\".");
                    break;
            }
        }

        private void ParseIdentifier() {
            // Read while there are letters, digits or underscores
            while (char.IsLetterOrDigit(Peek()) || Peek() == '_') Advance();

            string text = source.Substring(start, current - start);

            // Check if the identifier is a reserved keyword
            switch (text) {
                case "var":
                    AddToken(TokenType.VAR);
                    break;
                case "for":
                    AddToken(TokenType.FOR);
                    break;
                case "end":
                    AddToken(TokenType.END);
                    break;
                case "in":
                    AddToken(TokenType.IN);
                    break;
                case "do":
                    AddToken(TokenType.DO);
                    break;
                case "read":
                    AddToken(TokenType.READ);
                    break;
                case "print":
                    AddToken(TokenType.PRINT);
                    break;
                case "int":
                    AddToken(TokenType.INT);
                    break;
                case "string":
                    AddToken(TokenType.STR);
                    break;
                case "bool":
                    AddToken(TokenType.BOOL);
                    break;
                case "assert":
                    AddToken(TokenType.ASSERT);
                    break;
                default:
                    AddToken(TokenType.IDENT);
                    break;
            }
        }

        private void ParseNumber() {
            // Read while there are digits
            while (char.IsDigit(Peek())) Advance();
            string literal = source.Substring(start, current - start);
            AddToken(TokenType.INTEGER, int.Parse(literal));
        }

        private void ParseString() {
            // Read characters until EOF or closing ".
            while (!IsEOF() && Peek() != '"') {
                // Remember to count lines
                if (Peek() == '\n') line++;
                Advance();
            }

            // Catch unterminated string
            if (IsEOF()) {
                Program.Error(line, "Unterminated string.");
                return;
            }

            // Consume the closing ".
            Advance();
            // Trim the surrounding quotes
            string value = source.Substring(start + 1, current - start - 2);
            AddToken(TokenType.STRING, value);
        }

        private void ParseMultilineComment() {
            Console.WriteLine(source);
            // Read characters until EOF or closing */.
            while (!IsEOF() && Peek() != '*' && PeekNext() != '/') {
                // Remember to count lines
                if (Peek() == '\n') line++;
                // Check if a new multiline comment is starting /*
                if (!IsEOF() && Peek() == '/' && PeekNext() == '*') {
                    Advance();
                    Advance();
                    ParseMultilineComment();
                } else {
                    Advance();
                }
            }

            // Catch unterminated comment
            if (IsEOF()) {
                Program.Error(line, "Unterminated multiline comment.");
                return;
            }

            // Consume the closing */
            Advance();
            Advance();
        }

        private char Advance() {
            current++;
            return source[current - 1];
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

            current++;
            return true;
        }

        private bool IsEOF() {
            return current >= source.Length;
        }

        private void AddToken(TokenType type) {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal) {
            string text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

    }
}
