namespace miniPL {
    public interface Visitor<T> {
        T VisitBinaryExpression(Binary expression);

        T VisitUnaryExpression(Unary expression);

        T VisitGroupingExpression(Grouping expression);

        T VisitLiteralExpression(Literal expression);

        T VisitPrintStatement(Print print);

        T VisitAssertStatement(Assert assert);

        T VisitReadStatement(Read read);

        T VisitVariableCreateStatement(VariableCreate variableCreate);

        T VisitVariableAssignStatement(VariableAssign variableAssign);

        T VisitForstatement(ForLoop forLoop);
    }
}
