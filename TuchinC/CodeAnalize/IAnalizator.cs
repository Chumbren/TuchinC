using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements;

namespace TuchinC.CodeAnalize
{
    public interface IAnalizator
    {
        void Analize(List<Stmt?> stmts);
        void Analize(Stmt? stmt);
        void Analize(Expr? expr);
    }
}
