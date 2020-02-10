namespace miniPL {
    public interface Expression { }

    public struct Group : Expression {
        readonly Expression expression;

        public Group(Expression expression) {
            this.expression = expression;
        }
    }

    public struct Binary : Expression {
        readonly Expression left;
        readonly Token op;
        readonly Expression right;

        public Binary(Expression left, Token op, Expression right) {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }

    public struct Unary : Expression {
        readonly Token op;
        readonly Expression right;

        public Unary(Token op, Expression right) {
            this.op = op;
            this.right = right;
        }
    }
}
