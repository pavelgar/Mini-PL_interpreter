namespace miniPL {
    public interface Expression { }

    public struct Binary : Expression {
        readonly Expression left;
        readonly Token op;
        readonly Expression right;

        public Binary(Expression left, Token op, Expression right) {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public override string ToString() {
            return $"Binary( {left} {op.type} {right} )";
        }
    }

    public struct Unary : Expression {
        readonly Token op;
        readonly Expression expr;

        public Unary(Token op, Expression expr) {
            this.op = op;
            this.expr = expr;
        }

        public override string ToString() {
            return $"Unary( {op.type} {expr} )";
        }
    }

    public struct Grouping : Expression {
        readonly Expression expr;
        public Grouping(Expression expr) {
            this.expr = expr;
        }

        public override string ToString() {
            return $"Grouping( {expr} )";
        }
    }

    public struct Literal : Expression {
        readonly object value;
        public Literal(object value) {
            this.value = value;
        }

        public override string ToString() {
            return $"Literal( {value} )";
        }
    }
}
