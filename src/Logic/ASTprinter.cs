using System.Text;

namespace miniPL {
    class AstPrinter : Visitor<string> {
        public string VisitBinaryExpression(Binary expression) {
            return Parenthesize(expression.op.rawValue, expression.left, expression.right);
        }

        public string VisitGroupingExpression(Grouping expression) {
            return Parenthesize("group", expression.expr);
        }

        public string VisitLiteralExpression(Literal expression) {
            return expression.value == null ? "null" : expression.value.ToString();
        }

        public string VisitUnaryExpression(Unary expression) {
            return Parenthesize(expression.op.rawValue, expression.expr);
        }

        string Print(Expression expression) {
            return expression.accept(this);
        }

        private string Parenthesize(string name, params Expression[] expressions) {
            StringBuilder builder = new StringBuilder($"({name}");

            foreach (Expression expression in expressions) {
                builder.Append(" ");
                builder.Append(expression.accept(this));
            }
            builder.Append(")");

            return builder.ToString();
        }
    }
}
