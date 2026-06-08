using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements.Visitors;

namespace TuchinC.AST.Nodes.Statements
{
    public record class Elif(Expr Condition, Stmt ThenBranch);
    public class If(Token keyword, Expr condition, Stmt thenBranch,List<Elif> elifBranches, Stmt? elseBranch) : Stmt(keyword)
    {
        public readonly Expr Condition = condition;
        public readonly Stmt ThenBranch = thenBranch;
        public readonly List<Elif> ElifBranches = elifBranches;
        public readonly Stmt? ElseBranch = elseBranch;

        public If(Token keyword, Expr condition, Stmt thenBranch) : this(keyword, condition, thenBranch, [],null)
        { }
        public If(Token keyword, Expr condition, Stmt thenBranch, List<Elif> elifBranches) : this(keyword, condition, thenBranch, elifBranches, null)
        { }

        public override void Accept(IVisitor visitor) => visitor.VisitIfStmt(this);
    }
}
