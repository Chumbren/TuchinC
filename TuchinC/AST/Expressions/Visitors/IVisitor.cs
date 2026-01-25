using System.Collections.Generic;
using TuchinC.AST.Expressions.Calles;



namespace TuchinC.AST.Expressions.Visitors {
    public interface IVisitor<T>
    {
        T VisitVariableExpr(Variable expr);
        T VisitAssignExpr(Assign expr);
        T VisitTernaryExpr(Ternary expr);
        T VisitLogicalExpr(Logical logical);
        T VisitBinaryExpr(Binary expr);
        T VisitUnaryExpr(Unary expr);
        T VisitCallExpr(Call expr);
        T VisitGetExpr(Get expr);
        T VisitSetExpr(Set expr);
        T VisitLiteralExpr(Literal expr);
        T VisitArrowFunction(ArrowFunction expr);
        T VisitCollectionExpr(Collection expr);
        T VisitGroupingExpr(Grouping expr);

        T VisitThisExpr(This expr);
    }

 
}