namespace miniPL {
    public interface Expression { }

    public struct Binary : Expression {
        readonly Operand left;
        readonly Token op;
        readonly Operand right;

        public Binary(Operand left, Token op, Operand right) {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public override string ToString() {
            return $"Binary( {left} {op.type} {right} )";
        }
    }

    public struct Unary : Expression {
        readonly Token? op;
        readonly Operand expr;

        public Unary(Token? op, Operand expr) {
            this.op = op;
            this.expr = expr;
        }

        public override string ToString() {
            if (op.HasValue) {
                return $"Unary( {op.Value.type} {expr} )";
            }
            return $"Unary( {expr} )";
        }
    }
}
