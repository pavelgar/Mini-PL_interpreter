namespace miniPL {
    public interface Statement { }

    public struct VariableCreate : Statement {
        readonly Token ident;
        readonly Token type;
        readonly Expression expression;

        public VariableCreate(Token ident, Token type, Expression expression) {
            this.ident = ident;
            this.type = type;
            this.expression = expression;
        }
    }

    public struct VariableAssign : Statement {
        readonly Token ident;
        readonly Expression expression;

        public VariableAssign(Token ident, Expression expression) {
            this.ident = ident;
            this.expression = expression;
        }
    }

    public struct ForLoop : Statement {
        readonly Token ident;
        readonly Expression start;
        readonly Expression end;

        readonly Statement[] statements;

        public ForLoop(
            Token ident,
            Expression start,
            Expression end,
            Statement[] statements
        ) {
            this.ident = ident;
            this.start = start;
            this.end = end;
            this.statements = statements;
        }
    }

    public struct Read : Statement {
        readonly Token ident;

        public Read(Token ident) {
            this.ident = ident;
        }
    }

    public struct Print : Statement {
        readonly Expression expression;

        public Print(Expression expression) {
            this.expression = expression;
        }
    }

    public struct Assert : Statement {
        readonly Expression expression;

        public Assert(Expression expression) {
            this.expression = expression;
        }
    }
}
