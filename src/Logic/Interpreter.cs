using System;

namespace miniPL {
    public class Interpreter : Visitor<object> {
        public void Interpret(Expression expression) {
            try {
                object value = Evaluate(expression);
                Console.WriteLine(Stringify(value));
            } catch (RuntimeError error) {
                Program.RuntimeError(error);
            }
        }

        private string Stringify(object obj) {
            if (obj == null) return "null";
            return obj.ToString();
        }

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
                    throw new RuntimeError(
                        expression.op,
                        "Both operands must be strings or numbers"
                    );

                case TokenType.SUB:
                    CheckForNumber(expression.op, left, right);
                    return (double) left - (double) right;

                case TokenType.MULT:
                    CheckForNumber(expression.op, left, right);
                    return (double) left * (double) right;

                case TokenType.DIV:
                    CheckForNumber(expression.op, left, right);
                    return (double) left / (double) right;

                case TokenType.LT:
                    CheckForNumber(expression.op, left, right);
                    return (double) left < (double) right;

                case TokenType.EQ:
                    return IsEqual(left, right);

                case TokenType.AND:
                    CheckForBoolean(expression.op, left, right);
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

        public object VisitPrintStatement(Print print) {
            throw new NotImplementedException();
        }

        public object VisitAssertStatement(Assert assert) {
            throw new NotImplementedException();
        }

        public object VisitReadStatement(Read read) {
            throw new NotImplementedException();
        }

        public object VisitVariableCreateStatement(VariableCreate variableCreate) {
            throw new NotImplementedException();
        }

        public object VisitVariableAssignStatement(VariableAssign variableAssign) {
            throw new NotImplementedException();
        }

        public object VisitForstatement(ForLoop forLoop) {
            throw new NotImplementedException();
        }

        private object Evaluate(Expression expression) {
            return expression.Accept(this);
        }

        private bool ConvertToBoolean(object obj) {
            if (obj == null) return false;
            // Tries to cast the evaluated expression to boolean
            if (obj is bool) return (bool) obj;
            return true;
        }

        private void CheckForNumber(Token op, object left, object right) {
            if (left is double && right is double) return;
            throw new RuntimeError(op, "Both operands must be numbers.");
        }

        private void CheckForBoolean(Token op, object left, object right) {
            if (left is bool && right is bool) return;
            throw new RuntimeError(op, "Both operands must be boolean.");
        }

        private bool IsEqual(object a, object b) {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }
    }
}
