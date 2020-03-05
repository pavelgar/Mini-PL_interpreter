using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace miniPL {
    public class Interpreter : Visitor<object> {
        Environment environment = new Environment();

        public void Interpret(List<Statement> statements) {
            try {
                foreach (Statement statement in statements) {
                    Execute(statement);
                }
            } catch (RuntimeError error) {
                Program.RuntimeError(error);
            }
        }

        private void Execute(Statement statement) {
            statement.Accept(this);
        }

        private object Evaluate(Expression expression) {
            return expression.Accept(this);
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
                        "Both operands must be of type string or integer."
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

        public object VisitUnaryExpression(Unary expression) {
            // Evaluate the expresion and check if there is a '!' before it.
            object value = Evaluate(expression.expr);
            if (expression.op.type == TokenType.NOT) {
                CheckForBoolean(expression.op, value);
                return (bool) value;
            }
            return null;
        }

        public object VisitGroupingExpression(Grouping expression) {
            // Evaluate the expression before returning it.
            return Evaluate(expression.expr);
        }

        public object VisitLiteralExpression(Literal expression) {
            // Return just the value. Nothing else to do.
            return expression.value;
        }

        public object VisitVariableExpression(Variable variable) {
            // Return the stored value associated with that identifier.
            return environment.Get(variable.ident);
        }

        public object VisitAssignmentExpression(Assignment assingment) {
            object value = Evaluate(assingment.expr);
            environment.Assign(assingment.ident, value);
            return null;
        }

        public object VisitPrintStatement(Print print) {
            object value = Evaluate(print.expression);
            if (value is string) {
                Console.Write(Regex.Unescape((string) value));
            } else Console.Write(value);
            return null;
        }

        public object VisitAssertStatement(Assert assert) {
            object value = Evaluate(assert.expression);
            CheckForBoolean(assert.token, value);
            if (!(bool) value) {
                throw new RuntimeError(assert.token, "Assertion failed.");
            }
            return (bool) value;
        }

        public object VisitReadStatement(Read read) {
            string input = Console.ReadLine().Split(null) [0];

            if (double.TryParse(input, out double i))
                environment.Assign(read.ident, i);
            else if (input.Equals("true"))
                environment.Assign(read.ident, true);
            else if (input.Equals("false"))
                environment.Assign(read.ident, false);
            else
                environment.Assign(read.ident, input);

            return null;
        }

        public object VisitVarStatement(Var var) {
            object value = var.expression == null ? null : Evaluate(var.expression);
            environment.Define(var.ident, value);
            return null;
        }

        public object VisitForStatement(ForLoop forLoop) {
            object start = Evaluate(forLoop.start);
            object end = Evaluate(forLoop.end);

            CheckForNumber(forLoop.range, start, end);
            environment.SetAsControl(forLoop.ident);

            for (double i = (double) start; i <= (double) end; i++) {
                environment.ControlAssign(forLoop.ident, i);
                foreach (Statement statement in forLoop.statements)
                    Execute(statement);
            }
            environment.RemoveFromControl(forLoop.ident);
            return null;
        }

        public object VisitExpressionStatement(ExpressionStmt expressionStmt) {
            Evaluate(expressionStmt.expression);
            return null;
        }

        private void CheckForNumber(Token op, object left, object right) {
            if (left is double && right is double) return;
            throw new RuntimeError(op, "Both operands must be of type integer.");
        }

        private void CheckForBoolean(Token op, object left, object right) {
            if (left is bool && right is bool) return;
            throw new RuntimeError(op, "Both operands must be of type bool.");
        }

        private void CheckForBoolean(Token op, object value) {
            if (value is bool) return;
            throw new RuntimeError(op, "The operand must be of type bool.");
        }

        private bool IsEqual(object a, object b) {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }
    }
}
