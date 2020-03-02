using System.Collections.Generic;

namespace miniPL {
    public interface Statement {
        public T Accept<T>(Visitor<T> visitor);
    }

    public struct Var : Statement {
        public readonly Token ident;
        public readonly Expression expression;

        public Var(Token ident, Expression expression) {
            this.ident = ident;
            this.expression = expression;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitVariableStatement(this);
        }

        public override string ToString() {
            return $"Variable( {ident.rawValue} : {ident.type} := {expression} )";
        }
    }

    public struct ForLoop : Statement {
        public readonly Token ident;
        public readonly Expression start;
        public readonly Expression end;

        public readonly List<Statement> statements;

        public ForLoop(
            Token ident,
            Expression start,
            Expression end,
            List<Statement> statements
        ) {
            this.ident = ident;
            this.start = start;
            this.end = end;
            this.statements = statements;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitForStatement(this);
        }

        public override string ToString() {
            return $"ForLoop( {ident.type} in {start} .. {end}:\n    {string.Join("\n    ", statements)}\n)";
        }
    }

    public struct Read : Statement {
        public readonly Token ident;

        public Read(Token ident) {
            this.ident = ident;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitReadStatement(this);
        }

        public override string ToString() {
            return $"Read( {ident.type} )";
        }
    }

    public struct Print : Statement {
        public readonly Expression expression;

        public Print(Expression expression) {
            this.expression = expression;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitPrintStatement(this);
        }

        public override string ToString() {
            return $"Print( {expression} )";
        }
    }

    public struct Assert : Statement {
        public readonly Expression expression;

        public Assert(Expression expression) {
            this.expression = expression;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitAssertStatement(this);
        }

        public override string ToString() {
            return $"Assert( {expression} )";
        }
    }

    public struct ExpressionStmt : Statement {
        public readonly Expression expression;

        public ExpressionStmt(Expression expression) {
            this.expression = expression;
        }

        public T Accept<T>(Visitor<T> visitor) {
            return visitor.VisitExpressionStatement(this);
        }

        public override string ToString() {
            return $"ExpressionStmt( {expression} )";
        }
    }
}
