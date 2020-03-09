namespace miniPL {
    public interface Expression {
        public T Accept<T>(Visitor<T> visitor);
    }

    public struct Binary : Expression {
        public readonly Expression left;
        public readonly Token op;
        public readonly Expression right;

        public Binary(Expression left, Token op, Expression right) {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitBinaryExpression(this);
        }
    }

    public struct Unary : Expression {
        public readonly Token op;
        public readonly Expression expr;

        public Unary(Token op, Expression expr) {
            this.op = op;
            this.expr = expr;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitUnaryExpression(this);
        }
    }

    public struct Grouping : Expression {
        public readonly Expression expr;
        public Grouping(Expression expr) {
            this.expr = expr;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitGroupingExpression(this);
        }
    }

    public struct Literal : Expression {
        public readonly object value;
        public Literal(object value) {
            this.value = value;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitLiteralExpression(this);
        }
    }

    public struct Variable : Expression {
        public readonly Token ident;
        public Variable(Token ident) {
            this.ident = ident;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitVariableExpression(this);
        }
    }

    public struct Assignment : Expression {
        public readonly Token ident;
        public readonly Expression expr;
        public Assignment(Token ident, Expression expr) {
            this.ident = ident;
            this.expr = expr;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitAssignmentExpression(this);
        }
    }
}
