using System.Text;

namespace miniPL {
    class ASTPrinter : Visitor<string> {

        private int indent;
        private readonly int increment;

        public ASTPrinter(int increment) {
            this.indent = 0;
            this.increment = increment;
        }

        public string Print(Statement[] statements) {
            return Parenthesize("Program", statements);
        }

        public string VisitBinaryExpression(Binary expression) {
            return Parenthesize($"Binary [{expression.op.rawValue}]", expression.left, expression.right);
        }

        public string VisitGroupingExpression(Grouping expression) {
            return Parenthesize("Group", expression.expr);
        }

        public string VisitLiteralExpression(Literal expression) {
            string value = expression.value == null ? "null" : expression.value.ToString();
            if (expression.value is string) {
                return $"Literal (\"{value}\")";
            }
            return $"Literal ({value})";
        }

        public string VisitUnaryExpression(Unary expression) {
            return Parenthesize($"Unary [{expression.op.rawValue}]", expression.expr);
        }

        public string VisitVariableExpression(Variable variable) {
            return $"Variable ({variable.ident.rawValue})";
        }

        public string VisitAssignmentExpression(Assignment assingment) {
            return Parenthesize($"Assignment [{assingment.ident.rawValue}]", assingment.expr);
        }

        public string VisitPrintStatement(Print print) {
            return Parenthesize("Print", print.expression);
        }

        public string VisitAssertStatement(Assert assert) {
            return Parenthesize("Assert", assert.expression);
        }

        public string VisitReadStatement(Read read) {
            return $"Read [{read.ident.rawValue}]";
        }

        public string VisitVarStatement(Var var) {
            return Parenthesize($"Var [{var.ident.rawValue} : {var.type.rawValue}]", var.expression);
        }

        public string VisitForStatement(ForLoop forLoop) {
            string title = $"ForLoop [{forLoop.ident.rawValue} in {forLoop.start.Accept(this)} .. {forLoop.end.Accept(this)}]";
            return Parenthesize(title, forLoop.statements);
        }

        public string VisitExpressionStatement(ExpressionStmt expressionStmt) {
            return Parenthesize("Expression", expressionStmt.expression);
        }

        private string Parenthesize(string name, params Expression[] expressions) {
            StringBuilder builder = new StringBuilder($"{name} (\n");
            indent += increment;

            foreach (Expression expression in expressions) {
                builder.Append(new string(' ', indent));
                builder.Append(expression.Accept(this));
                builder.Append('\n');
            }

            indent -= increment;
            builder.Append(new string(' ', indent) + ")");

            return builder.ToString();
        }

        private string Parenthesize(string name, params Statement[] statements) {
            StringBuilder builder = new StringBuilder($"{name} (\n");
            indent += increment;

            foreach (Statement statement in statements) {
                builder.Append(new string(' ', indent));
                builder.Append(statement.Accept(this));
                builder.Append('\n');
            }

            indent -= increment;
            builder.Append(new string(' ', indent) + ")");

            return builder.ToString();
        }
    }
}
