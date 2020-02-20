using System;
using System.Collections.Generic;

namespace miniPL {
    public class Parser {
        private readonly Scanner scanner;
        private Token currentToken;
        private Token nextToken;

        public Parser(Scanner scanner) {
            // Set the scanner and read the first token.
            this.scanner = scanner;
            nextToken = this.scanner.ScanToken();
        }

        internal List<Statement> Execute() {
            List<Statement> statements = new List<Statement>();
            while (
                nextToken.type != TokenType.EOF &&
                nextToken.type != TokenType.END
            ) {
                statements.Add(Stmt());
                Match(TokenType.SEMICOLON);
            }
            return statements;
        }

        private Statement Stmt() {
            currentToken = nextToken;
            nextToken = scanner.ScanToken();

            switch (currentToken.type) {
                case TokenType.VAR:
                    return ParseVariableCreate();

                case TokenType.IDENT:
                    return ParseVariableAssign();

                case TokenType.FOR:
                    return ParseForLoop();

                case TokenType.READ:
                    Match(TokenType.IDENT);
                    return new Read(currentToken);

                case TokenType.PRINT:
                    return new Print(Expr());

                case TokenType.ASSERT:
                    return ParseAssert();

                default:
                    throw new System.Exception($"Statement cannot start with {currentToken}");
            }
        }

        private Statement ParseAssert() {
            Match(TokenType.LEFT_PAREN);
            Expression expr = Expr();
            Match(TokenType.RIGHT_PAREN);
            return new Assert(expr);
        }

        private Statement ParseForLoop() {
            Match(TokenType.IDENT);
            Token ident = currentToken;
            Match(TokenType.IN);
            Expression start = Expr();
            Match(TokenType.RANGE);
            Expression end = Expr();
            Match(TokenType.DO);
            List<Statement> stmts = Execute();
            Match(TokenType.END);
            Match(TokenType.FOR);
            return new ForLoop(ident, start, end, stmts);
        }

        private Statement ParseVariableAssign() {
            Token ident = currentToken;
            Match(TokenType.ASSIGN);
            Expression expr = Expr();
            return new VariableAssign(ident, expr);
        }

        private Statement ParseVariableCreate() {
            Match(TokenType.IDENT);
            Token ident = currentToken;
            Match(TokenType.COLON);
            TokenType[] acceptedTypes = { TokenType.INT, TokenType.STR, TokenType.BOOL };
            Match(acceptedTypes);
            Token type = currentToken;
            Expression expr = null;
            if (nextToken.type != TokenType.SEMICOLON) {
                Match(TokenType.ASSIGN);
                expr = Expr();
            }
            return new VariableCreate(ident, type, expr);
        }

        private Expression Expr() {
            if (nextToken.type == TokenType.NOT) {
                Match(TokenType.NOT);
                return new Unary(currentToken, Opnd());
            }
            Operand left = Opnd();

            if (nextToken.type == TokenType.SEMICOLON ||
                nextToken.type == TokenType.RANGE ||
                nextToken.type == TokenType.DO) {
                return new Unary(null, left);
            }

            TokenType[] acceptedOperators = {
                TokenType.ADD,
                TokenType.SUB,
                TokenType.MULT,
                TokenType.DIV,
                TokenType.LT,
                TokenType.EQ,
                TokenType.AND
            };
            Match(acceptedOperators);
            Token op = currentToken;
            Operand right = Opnd();
            return new Binary(left, op, right);
        }

        private Operand Opnd() {
            switch (nextToken.type) {
                case TokenType.INTEGER:
                    Match(TokenType.INTEGER);
                    return new Integer(currentToken);

                case TokenType.STRING:
                    Match(TokenType.STRING);
                    return new String(currentToken);

                case TokenType.IDENT:
                    Match(TokenType.IDENT);
                    return new Identifier(currentToken);

                case TokenType.LEFT_PAREN:
                    Match(TokenType.LEFT_PAREN);
                    Expression expr = Expr();
                    Match(TokenType.RIGHT_PAREN);
                    return new Expr(expr);

                default:
                    throw new Exception($"Unexpected token: {currentToken}");
            }
        }

        private bool Match(TokenType[] types) {
            foreach (TokenType type in types) {
                if (nextToken.type == type) {
                    currentToken = nextToken;
                    nextToken = scanner.ScanToken();
                    return true;
                }
            }
            throw new Exception($"Unexpected token: {currentToken}");
        }

        private bool Match(TokenType type) {
            if (nextToken.type == type) {
                currentToken = nextToken;
                nextToken = scanner.ScanToken();
                return true;
            }
            throw new Exception($"Unexpected token: {currentToken}");
        }
    }
}
