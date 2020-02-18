namespace miniPL {
    public interface Operand { }

    public struct Integer : Operand {
        readonly Token opnd;

        public Integer(Token opnd) {
            this.opnd = opnd;
        }
    }
    public struct String : Operand {
        readonly Token opnd;

        public String(Token opnd) {
            this.opnd = opnd;
        }
    }
    public struct Identifier : Operand {
        readonly Token opnd;

        public Identifier(Token opnd) {
            this.opnd = opnd;
        }
    }
    public struct Expr : Operand {
        readonly Expression expr;

        public Expr(Expression expr) {
            this.expr = expr;
        }
    }
}
