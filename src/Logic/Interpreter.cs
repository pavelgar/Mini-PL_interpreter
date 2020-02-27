namespace miniPL {
    public class Interpreter : Visitor<object> {
        public object VisitBinaryExpression(Binary expression) {
            object left = Evaluate(expression.left);
            object right = Evaluate(expression.right);

            switch (expression.op.type) {
                case TokenType.ADD:
                    if (left is double && right is double) {
                        return (double) left + (double) right;
                    }
                    if (left is string && right is string) {
                        return (string) left + (string) right;
                    }
                    return null;

                case TokenType.SUB:
                    return (double) left - (double) right;

                case TokenType.MULT:
                    return (double) left * (double) right;

                case TokenType.DIV:
                    return (double) left / (double) right;

                case TokenType.LT:
                    return (double) left < (double) right;

                case TokenType.EQ:
                    return IsEqual(left, right);

                case TokenType.AND:
                    return (bool) left && (bool) right;

                default:
                    return null;
            }
        }

        public object VisitGroupingExpression(Grouping expression) {
            // Evaluate the expression before returning it.
            return Evaluate(expression);
        }

        public object VisitLiteralExpression(Literal expression) {
            // Return just the value. Nothing else to do.
            return expression.value;
        }

        public object VisitUnaryExpression(Unary expression) {
            // Evaluate the expresion and check if there is a '!' before it.
            object expr = Evaluate(expression.expr);
            if (expression.op.type == TokenType.NOT) {
                return !ConvertToBoolean(expr);
            }
            return null;
        }

        public object Evaluate(Expression expression) {
            return expression.accept(this);
        }

        private bool ConvertToBoolean(object obj) {
            if (obj == null) return false;
            // Tries to cast the evaluated expression to boolean
            if (obj is bool) return (bool) obj;
            return true;
        }

        private bool IsEqual(object a, object b) {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }
    }
}
