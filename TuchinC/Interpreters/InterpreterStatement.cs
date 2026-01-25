using TuchinC.Exceptions;
using TuchinC.AST.Expressions.Visitors;
using TuchinC.Lexical;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Statements;
using TuchinC.AST.Statements.Visitors;
using TuchinC.AST.Expressions;
using Environment = TuchinC.Semantic.Environment;
using TuchinC.Objects.Callable;
using TuchinC.Objects.Callable.ClassInstance;


namespace TuchinC.Interpreters
{
    public partial class Interpreter : IVisitor
    {
        public void VisitLoopStmt(Loop stmt)
        {
            while (true)
            {
                if (stmt.Condition is not null && !IsTruthy(Evaluate(stmt.Condition)))
                    break;
                Execute(stmt.Body);
            }
        }

        public void VisitBlockStmt(Block stmt) => ExecuteBlock(stmt.Statements,
            new Environment(_environment));



        public void VisitExpressionStmt(Expression stmt) => Evaluate(stmt.Value);


        public void VisitLetStmt(Let stmt)
        {
            object? value = null;
            if (stmt.Initializer != null)
                value = Evaluate(stmt.Initializer);

            _environment.Define(stmt.Name.Lexeme, value);
        }



        public void VisitIfStmt(If stmt)
        {
            if (IsTruthy(Evaluate(stmt.Condition)))
                Execute(stmt.ThenBranch);
            else
            {
                foreach (var elif in stmt.ElifBranches)
                {
                    if (IsTruthy(Evaluate(elif.Condition)))
                    {
                        Execute(elif.ThenBranch);
                        return;
                    }
                }

                if (stmt.ElseBranch != null)
                    Execute(stmt.ElseBranch);
            }
        }

        internal void ExecuteBlock(List<Stmt?> statements,
            Environment environment)
        {
            Environment previous = _environment;
            try
            {
                _environment = environment;
                foreach (Stmt? statement in statements)
                    Execute(statement);
            }
            finally
            {
                _environment = previous;
            }
        }


       
        public void VisitFunctionStmt(Function stmt)
        {
            LoxFunction function = new(stmt,_environment, FunctionType.FUNCTION);
            _environment.Define(stmt.Name.Lexeme,function);
        }

        public void VisitReturnStmt(Return stmt)
        {
            object? value = null;
            if (stmt.Value != null) 
                value = Evaluate(stmt.Value);
            throw new Returned(value);
        }

        private void Execute(Stmt? stmt) => stmt?.Accept(this);

        public void VisitImportStmt(Use stmt) 
        { }

        public void VisitSwitchStmt(Switch stmt)
        {
            object? condition = Evaluate(stmt.Condition);
            var result = stmt.RunStrategy(condition);

            if (result is Block block)
                ExecuteBlock(block.Statements, _environment);
            else if (result is If @if)
                Execute(@if);
        }

        public void VisitClassStmt(Struct stmt)
        {
            _environment.Define(stmt.Name.Lexeme,null);

            Dictionary<string, LoxFunction> methods = [];

            foreach (var function in stmt.Body)
            {
                FunctionType type = function.Name.Lexeme == "init"? FunctionType.INITIALIZER :FunctionType.METHOD;
                LoxFunction method = new(function,_environment,type);
                methods.Add(function.Name.Lexeme,method);
            }

            LoxClass @struct = new(stmt.Name.Lexeme, methods);
            _environment.Assign(stmt.Name,@struct);
        }
    }
}
