using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements;
using TuchinC.AST.Nodes.Statements.Visitors;
using TuchinC.AST.Semantic.Types.Cast.TypesBinnary;
using TuchinC.AST.Semantic.Types.Exceptions;

namespace TuchinC.AST.Semantic.Types.Analizator
{
    internal partial class TypeAnalizator : IVisitor
    {
        public void Analize(Stmt? stmt) => stmt?.Accept(this);

        public void VisitBlockStmt(Block stmt)
        {
            foreach (var statement in stmt.Statements)
            {
                if(statement != null)
                   Analize(statement);
            }
        }

        public void VisitClassStmt(Struct stmt)
        {
            throw new NotImplementedException();
        }

        public void VisitExpressionStmt(Expression stmt) => Analize(stmt.Value);

        public void VisitFunctionStmt(Nodes.Statements.Function stmt)
        {
            throw new NotImplementedException();
        }

        public void VisitIfStmt(If stmt)
        {
            int index = EmitWait();
            Analize(stmt.Condition);
            var condition = Index(index);
            if(!condition.IsBoolean())
                Tuchin.Error(stmt.Keyword,
                  new CastBinnaryException(condition, ValueType.ToBoolean()));
        }

        public void VisitImportStmt(Use stmt)
        {
            throw new NotImplementedException();
        }

        public void VisitLetStmt(Let stmt)
        {
            if(stmt.Initializer != null)
            {
                int index = EmitWait();
                Analize(stmt.Initializer);
                var init = Index(index);

                if (!CastDeclaration.CanCast(stmt.Type, init.Type))
                    throw new CastBinnaryException(init, new ValueType(stmt.Type));
            }

            AddVaribleType(stmt.Name.Lexeme, stmt.Type);
        }

        public void VisitLoopStmt(Loop stmt)
        {
            int index = EmitWait();
            Analize(stmt.Condition);
            var condition = Index(index);

            if (!condition.IsBoolean())
                Tuchin.Error(stmt.Keyword,
                  new CastBinnaryException(condition, ValueType.ToBoolean()));

            Analize(stmt.Body);

        }

        public void VisitReturnStmt(Return stmt)
        {
            throw new NotImplementedException();
        }

        public void VisitSwitchStmt(Switch stmt)
        {
            throw new NotImplementedException();
        }

        public void VisitPrintStmt(Print stmt)
        {
            var index = EmitWait();
            Analize(stmt.Expression);

            var value = Index(index);

            if (!value.IsString())
               throw new CastBinnaryException(value, ValueType.GetToString());
        }

        void IVisitor.VisitFunctionStmt(Nodes.Statements.Function stmt)
        {
            throw new NotImplementedException();
        }
    }
}
