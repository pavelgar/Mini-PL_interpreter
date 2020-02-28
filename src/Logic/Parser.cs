using System.Collections.Generic;

namespace miniPL {
    public class Parser {
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens) {
            this.tokens = tokens;
        }

        internal List<Statement> Stmts() {
            List<Statement> statements = new List<Statement>();
            while (
                Peek().type != TokenType.EOF &&
                Peek().type != TokenType.END
            ) {
                statements.Add(Stmt());
                Match(TokenType.SEMICOLON);
            }
            return statements;
        }

        private Statement Stmt() {
            switch (tokens[current].type) {
                case TokenType.VAR:
                    return ParseVariableCreate();

                case TokenType.IDENT:
                    return ParseVariableAssign();

                case TokenType.FOR:
                    return ParseForLoop();

                case TokenType.READ:
                    Match(TokenType.IDENT);
                    return new Read(tokens[current]);

                case TokenType.PRINT:
                    return new Print(Expr());

                case TokenType.ASSERT:
                    return ParseAssert();

                default:
                    throw Error(
                        tokens[current],
                        $"Statement cannot start with {tokens[current]}"
                    );
            }
        }

        public Expression Parse() {
            try {
                return Expr();
            } catch (ParseError error) {
                return null;
            }
        }

        private void Sync() {
            Advance();

            while (!IsEnd()) {
                if (Previous().type == TokenType.SEMICOLON) return;

                switch (Peek().type) {
                    case TokenType.VAR:
                    case TokenType.IDENT:
                    case TokenType.FOR:
                    case TokenType.READ:
                    case TokenType.PRINT:
                    case TokenType.ASSERT:
                        return;
                }
            }
            Advance();
        }

        private Statement ParseAssert() {
            Match(TokenType.LEFT_PAREN);
            Expression expr = Expr();
            Match(TokenType.RIGHT_PAREN);
            return new Assert(expr);
        }

        private Statement ParseForLoop() {
            Match(TokenType.IDENT);
            Token ident = tokens[current];
            Match(TokenType.IN);
            Expression start = Expr();
            Match(TokenType.RANGE);
            Expression end = Expr();
            Match(TokenType.DO);
            List<Statement> stmts = Stmts();
            Match(TokenType.END);
            Match(TokenType.FOR);
            return new ForLoop(ident, start, end, stmts);
        }

        private Statement ParseVariableAssign() {
            Token ident = tokens[current];
            Match(TokenType.ASSIGN);
            Expression expr = Expr();
            return new VariableAssign(ident, expr);
        }

        private Statement ParseVariableCreate() {
            Match(TokenType.IDENT);
            Token ident = tokens[current];
            Match(TokenType.COLON);
            Match(new TokenType[] { TokenType.INT, TokenType.STR, TokenType.BOOL });
            Token type = tokens[current];
            Expression expr = null;
            if (Peek().type != TokenType.SEMICOLON) {
                Match(TokenType.ASSIGN);
                expr = Expr();
            }
            return new VariableCreate(ident, type, expr);
        }

        private Expression Expr() {
            return Equality();
        }

        private Expression Equality() {
            Expression expr = Comparison();

            while (Match(TokenType.EQ, TokenType.AND)) {
                Token op = Previous();
                Expression right = Comparison();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Expression Comparison() {
            Expression expr = Addition();

            while (Match(TokenType.LT)) {
                Token op = Previous();
                Expression right = Addition();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Expression Addition() {
            Expression expr = Multiplication();

            while (Match(TokenType.ADD, TokenType.SUB)) {
                Token op = Previous();
                Expression right = Multiplication();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Expression Multiplication() {
            Expression expr = Unary();

            while (Match(TokenType.MULT, TokenType.DIV)) {
                Token op = Previous();
                Expression right = Unary();
                expr = new Binary(expr, op, right);
            }
            return expr;
        }

        private Expression Unary() {
            if (Match(TokenType.NOT)) {
                Token op = Previous();
                Expression right = Unary();
                return new Unary(op, right);
            }

            return Primary();
        }

        private Expression Primary() {
            if (Match(TokenType.TRUE)) return new Literal(true);
            if (Match(TokenType.FALSE)) return new Literal(false);

            if (Match(TokenType.INTEGER, TokenType.STRING)) {
                return new Literal(Previous().literal);
            }

            // TODO: WHAT TO DO WITH IDENTIFIERS???

            if (Match(TokenType.LEFT_PAREN)) {
                Expression expr = Expr();
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression.");
                return new Grouping(expr);
            }

            throw Error(Peek(), "Expected expression.");
        }

        private Token Consume(TokenType type, string message) {
            if (Check(type)) return Advance();
            throw Error(Peek(), message);
        }

        private ParseError Error(Token token, string message) {
            Program.Error(token, message);
            return new ParseError();
        }

        private bool Match(params TokenType[] types) {
            foreach (TokenType type in types) {
                if (Check(type)) {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private Token Advance() {
            if (!IsEnd()) current++;
            return Previous();
        }

        private bool Check(TokenType type) {
            if (IsEnd()) return false;
            return Peek().type == type;
        }

        private bool IsEnd() {
            return Peek().type == TokenType.EOF;
        }

        private Token Peek() {
            return tokens[current];
        }

        private Token Previous() {
            return tokens[current - 1];
        }
    }
}
