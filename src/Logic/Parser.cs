using System.Collections.Generic;

namespace miniPL {
    public class Parser {
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens) {
            this.tokens = tokens;
        }

        public List<Statement> Parse() {
            List<Statement> statements = new List<Statement>();

            while (!IsEnd())
                statements.Add(Stmt());

            return statements;
        }

        private Statement Stmt() {
            try {
                if (Match(TokenType.VAR)) return VarStatement();
                if (Match(TokenType.FOR)) return ForStatement();
                if (Match(TokenType.READ)) return ReadStatement();
                if (Match(TokenType.PRINT)) return PrintStatement();
                if (Match(TokenType.ASSERT)) return AssertStatement();

                return ExpressionStatement();

            } catch (ParseError) {
                Sync();
                return null;
            }
        }

        private Statement VarStatement() {
            Token name = Consume(TokenType.IDENT, "Expected variable name.");
            Consume(TokenType.COLON, "Expected ':' after variable name.");
            // Just check for the type declarations for now...
            // TODO: Actually use the type declarations in variable creation
            Match(TokenType.STR, TokenType.BOOL, TokenType.INT);

            Expression expression = null;
            if (Match(TokenType.ASSIGN)) {
                expression = Expr();
            }

            Consume(TokenType.SEMICOLON, "Expected ';' after variable declaration.");
            return new Var(name, expression);
        }

        private Statement ForStatement() {
            Token ident = Consume(TokenType.IDENT, "Expected variable name.");
            Consume(TokenType.IN, "Expected 'in' after variable name.");

            Expression start = Expr();
            Token range = Consume(TokenType.RANGE, "Expected '..' after start-of-range value.");
            Expression end = Expr();
            Consume(TokenType.RANGE, "Expected 'do' after end-of-range value.");

            List<Statement> statements = new List<Statement>();
            while (!Check(TokenType.END) && !IsEnd()) {
                statements.Add(Stmt());
            }

            Consume(TokenType.FOR, "Expected 'for' after 'end' keyword.");
            Consume(TokenType.SEMICOLON, "Expected ';' after expression.");

            return new ForLoop(ident, start, range, end, statements);
        }

        private Statement ReadStatement() {
            Token name = Consume(TokenType.IDENT, "Expected variable name.");
            Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
            return new Read(name);
        }

        private Statement PrintStatement() {
            Expression expression = Expr();
            Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
            return new Print(expression);

        }

        private Statement AssertStatement() {
            Expression expression = Expr();
            Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
            return new Assert(Previous(), expression);
        }

        private Statement ExpressionStatement() {
            Expression expression = Expr();
            Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
            return new ExpressionStmt(expression);
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

            if (Match(TokenType.INTEGER, TokenType.STRING))
                return new Literal(Previous().literal);

            if (Match(TokenType.IDENT))
                return new Variable(Previous());

            if (Match(TokenType.LEFT_PAREN)) {
                Expression expr = Expr();
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression.");
                return new Grouping(expr);
            }

            throw Error(Peek(), "Expected grouping, literal or indentifier.");
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
