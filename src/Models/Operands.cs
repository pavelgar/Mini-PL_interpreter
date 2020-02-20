namespace miniPL {
    public interface Operand { }

    public struct Integer : Operand {
        readonly Token opnd;

        public Integer(Token opnd) {
            this.opnd = opnd;
        }

        public override string ToString() {
            return $"Integer( {opnd.type} )";
        }
    }
    public struct String : Operand {
        readonly Token opnd;

        public String(Token opnd) {
            this.opnd = opnd;
        }

        public override string ToString() {
            return $"String( {opnd.type} )";
        }
    }
    public struct Identifier : Operand {
        readonly Token opnd;

        public Identifier(Token opnd) {
            this.opnd = opnd;
        }

        public override string ToString() {
            return $"Identifier( {opnd.type} )";
        }
    }
    public struct Expr : Operand {
        readonly Expression expr;

        public Expr(Expression expr) {
            this.expr = expr;
        }

        public override string ToString() {
            return $"Expr( {expr} )";
        }
    }
}
