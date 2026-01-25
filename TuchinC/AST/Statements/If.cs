using TuchinC.AST.Expressions;
using TuchinC.AST.Statements.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Statements
{
    public record class Elif(Expr Condition, Stmt? ThenBranch);
    public class If(Expr condition, Stmt thenBranch,List<Elif> elifBranches, Stmt? elseBranch) : Stmt
    {
        public readonly Expr Condition = condition;
        public readonly Stmt ThenBranch = thenBranch;
        public readonly List<Elif> ElifBranches = elifBranches;
        public readonly Stmt? ElseBranch = elseBranch;

        public If(Expr condition, Stmt thenBranch) : this(condition, thenBranch, [],null)
        { }
        public If(Expr condition, Stmt thenBranch, List<Elif> elifBranches) : this(condition, thenBranch, elifBranches,null)
        { }

        public override void Accept(IVisitor visitor) => visitor.VisitIfStmt(this);
    }
}
