using TuchinC.AST.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Statements.Visitors
{
    public interface IVisitor
    {
        void VisitExpressionStmt(Expression stmt);

        void VisitReturnStmt(Return stmt);
        void VisitImportStmt(Use stmt);
        void VisitFunctionStmt(Function stmt);
        void VisitIfStmt(If stmt);
        void VisitClassStmt(Struct stmt);
        void VisitSwitchStmt(Switch stmt);
        void VisitLoopStmt(Loop stmt);

        void VisitLetStmt(Let stmt);
        void VisitBlockStmt(Block stmt);
    }
}
