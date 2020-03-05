using System;
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

        public string VisitVariableExpression(Variable variable) {
            throw new NotImplementedException();
        }

        public string VisitAssignmentExpression(Assignment assingment) {
            throw new NotImplementedException();
        }

        public string VisitPrintStatement(Print print) {
            throw new NotImplementedException();
        }

        public string VisitAssertStatement(Assert assert) {
            throw new NotImplementedException();
        }

        public string VisitReadStatement(Read read) {
            throw new NotImplementedException();
        }

        public string VisitVarStatement(Var var) {
            throw new NotImplementedException();
        }

        public string VisitForStatement(ForLoop forLoop) {
            throw new NotImplementedException();
        }

        public string VisitExpressionStatement(ExpressionStmt expressionStmt) {
            throw new NotImplementedException();
        }

        string Print(Expression expression) {
            return expression.Accept(this);
        }

        private string Parenthesize(string name, params Expression[] expressions) {
            StringBuilder builder = new StringBuilder($"({name}");

            foreach (Expression expression in expressions) {
                builder.Append(" ");
                builder.Append(expression.Accept(this));
            }
            builder.Append(")");

            return builder.ToString();
        }
    }
}
