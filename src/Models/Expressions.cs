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
        readonly Token? not;
        readonly Operand expr;

        public Unary(Token? not, Operand expr) {
            this.not = not;
            this.expr = expr;
        }

        public override string ToString() {
            if (not.HasValue) {
                return $"Unary( {not.Value.type} {expr} )";
            }
            return $"Unary( {expr} )";
        }
    }
}
