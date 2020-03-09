namespace miniPL {
    public interface Statement {
        public T Accept<T>(Visitor<T> visitor);
    }

    public struct Var : Statement {
        public readonly Token ident;
        public readonly Token type;
        public readonly Expression expression;

        public Var(Token ident, Token type, Expression expression) {
            this.ident = ident;
            this.type = type;
            this.expression = expression;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitVarStatement(this);
        }
    }

    public struct ForLoop : Statement {
        public readonly Token ident;
        public readonly Expression start;
        public readonly Token range;
        public readonly Expression end;
        public readonly Statement[] statements;

        public ForLoop(
            Token ident,
            Expression start,
            Token range,
            Expression end,
            Statement[] statements
        ) {
            this.ident = ident;
            this.start = start;
            this.range = range;
            this.end = end;
            this.statements = statements;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitForStatement(this);
        }
    }

    public struct Read : Statement {
        public readonly Token ident;

        public Read(Token ident) {
            this.ident = ident;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitReadStatement(this);
        }
    }

    public struct Print : Statement {
        public readonly Expression expression;

        public Print(Expression expression) {
            this.expression = expression;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitPrintStatement(this);
        }
    }

    public struct Assert : Statement {
        public readonly Token token;
        public readonly Expression expression;

        public Assert(Token token, Expression expression) {
            this.token = token;
            this.expression = expression;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitAssertStatement(this);
        }
    }

    public struct ExpressionStmt : Statement {
        public readonly Expression expression;

        public ExpressionStmt(Expression expression) {
            this.expression = expression;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitExpressionStatement(this);
        }
    }
}
