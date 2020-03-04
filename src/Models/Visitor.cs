namespace miniPL {
    public interface Visitor<T> {
        T VisitBinaryExpression(Binary expression);

        T VisitUnaryExpression(Unary expression);

        T VisitGroupingExpression(Grouping expression);

        T VisitLiteralExpression(Literal expression);

        T VisitVariableExpression(Variable variable);

        T VisitPrintStatement(Print print);

        T VisitAssertStatement(Assert assert);

        T VisitReadStatement(Read read);

        T VisitVarStatement(Var var);

        T VisitForStatement(ForLoop forLoop);

        T VisitExpressionStatement(ExpressionStmt expressionStmt);
    }
}
