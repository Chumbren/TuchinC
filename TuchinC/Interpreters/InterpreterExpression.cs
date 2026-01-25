using TuchinC.AST.Expressions;
using TuchinC.AST.Expressions.Calles;
using TuchinC.AST.Statements;
using TuchinC.Exceptions;
using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.Objects.Callable;
using TuchinC.Objects.Callable.ClassInstance;
using TuchinC.AST.Expressions.Visitors;

namespace TuchinC.Interpreters
{
    public partial class Interpreter : IVisitor<object?>
    {
        public object? VisitBinaryExpr(Binary expr)
        {
            object? left = Evaluate(expr.Left);
            object? right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.GREATER:
                    if (left is null || right is null) break;
                    CheckNumberOperands(expr.Operator, left, right);

                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:

                    if (left is null || right is null) break;
                    CheckNumberOperands(expr.Operator, left, right);

                    return (double)left >= (double)right;
                case TokenType.LESS:

                    if (left is null || right is null) break;
                    CheckNumberOperands(expr.Operator, left, right);

                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:

                    if (left is null || right is null) break;
                    CheckNumberOperands(expr.Operator, left, right);

                    return (double)left <= (double)right;
                case TokenType.BANG_EQUAL: return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL: return IsEqual(left, right);
                case TokenType.MINUS:

                    if (left == null || right == null) break;

                    return (double)left - (double)right;
                case TokenType.PLUS:

                    if (left is null || right is null) break;

                    if (left is double loper && right is double roper)
                    {
                        return loper + roper;
                    }

                    if (left is string lstr && right is string rstr)
                    {
                        return lstr + rstr;
                    }

                    if (left is string || right is string)
                    {
                        if (left is string ls && right is double) return $"{ls}{right}";
                        if (right is string rs && left is double) return $"{rs}{left}";
                    }

                    break;
                case TokenType.SLASH:

                    if (left is null || right is null) break;
                    CheckNumberOperands(expr.Operator, left, right);


                    if ((double)right == 0)
                        throw new RuntimeError(expr.Operator, "devined on zero");

                    return (double)left / (double)right;
                case TokenType.STAR:

                    if (left is null || right is null) break;
                    CheckNumberOperands(expr.Operator, left, right);

                    return (double)left * (double)right;
                case TokenType.LEFT_OFFSET:

                    if (left is null || right is null) break;
                    CheckNumberOperands(expr.Operator, left, right);

                    return (int)(double)left << (int)(double)right;
                case TokenType.RIGHT_OFFSET:

                    if (left is null || right is null) break;
                    CheckNumberOperands(expr.Operator, left, right);

                    return (int)(double)left >> (int)(double)right;
                case TokenType.AND_BIT:

                    if (left is null || right is null) break;
                    CheckNumberOperands(expr.Operator, left, right);

                    return (int)(double)left & (int)(double)right;
                case TokenType.VLINE:

                    if (left is null || right is null) break;
                    CheckNumberOperands(expr.Operator, left, right);

                    return (int)(double)left | (int)(double)right;
                case TokenType.XOR:

                    if (left is null || right is null) break;
                    CheckNumberOperands(expr.Operator, left, right);

                    return (int)(double)left ^ (int)(double)right;
            }

            return null;
        }

        public object? VisitLogicalExpr(Logical expr)
        {

            object? left = Evaluate(expr.Left);
            if (expr.Operator.Type == TokenType.VLINE_VLINE)
            {
                if (IsTruthy(left)) return left;
            }
            else
            {
                if (!IsTruthy(left)) return left;
            }


            return Evaluate(expr.Right);
        }

        public object? VisitTernaryExpr(Ternary expr)
        {
            object? condition = Evaluate(expr.If);

            return Evaluate(IsTruthy(condition) ? expr.That : expr.Else);
        }

        public object? VisitVariableExpr(Variable expr) 
            => LookUpVariable(expr.Name,expr);


        private object? LookUpVariable(Token name, Expr expr)
        {

            if (_locals.TryGetValue(expr,out int? distance) && distance != null)
            {
                return _environment.GetAt(distance,name.Lexeme);
            }
            else
            {
                return Globals.Get(name);
            }
        }


        public object? VisitGroupingExpr(Grouping expr) => Evaluate(expr.Expression);

        public object? VisitLiteralExpr(Literal expr) => expr.Value;



        public object? VisitUnaryExpr(Unary expr)
        {
            object? right = Evaluate(expr.Right);
            if (right is null) return null;

            switch (expr.Operator.Type)
            {
                case TokenType.NOT_BIT:
                    CheckNumberOperand(expr.Operator, right);
                    return ~(int)(double)right;
                case TokenType.MINUS:
                    CheckNumberOperand(expr.Operator, right);
                    return -(double)right;
                case TokenType.PLUS:
                    CheckNumberOperand(expr.Operator, right);
                    return +(double)right;
                case TokenType.BANG: return !IsTruthy(right);
                default:
                    return null;
            }
        }

        public object? VisitCallExpr(Call expr)
        {
            
            object? callee = Evaluate(expr.Calle);
            if (callee is not ILoxCallable callable)
                throw new RuntimeError(expr.Paren,
                    "Вызов производится только с коллекциями, функциями или с классами");


            if (expr is FunctionCall function)
            {
                return FunctionCall(function, callable);
            }
            else if (expr is IteratorCall iterator)
            {
                return IteratorCall(iterator,(LoxCollection)callable);
            }

            throw new RuntimeError(expr.Paren,
                   "Вызов производится только с функциями или с классами");

        }

        private object? FunctionCall(FunctionCall expr,ILoxCallable function)
        {
            List<object?> args = [];
            foreach (Expr argument in expr.Arguments)
                args.Add(Evaluate(argument));


            if (args.Count != function.Arity())
            {
                throw new RuntimeError(expr.Paren,
                    $"Ожидается {function.Arity()} aргументов но переданно {args.Count}.");
            }
            return function?.Call(this, args);
        }

        private object? IteratorCall(IteratorCall expr, LoxCollection collection) 
        {
            object? arg = Evaluate(expr.Index);


            if (arg is not double || (arg is double iter && iter % 1 != 0))
                throw new RuntimeError(expr.Paren, "Аргумент итератора должен быть числом");

            int index = Convert.ToInt32((double)arg);

            if(index < 0)
                throw new RuntimeError(expr.Paren, "Аргумент итератора должен быть отрицательным числом");


            if (collection.Count() <= index)
                throw new RuntimeError(expr.Paren, "Индекс вышел за границы массива");


            return collection.Call(this, [index]);
        }

        public object? VisitArrowFunction(ArrowFunction expr)
        {
            int i = 0;
            while (_environment.Get($"ArrowFunction{++i}<>") != null) ;

            string name = $"ArrowFunction{i}<>";
            Token token = new(TokenType.IDENTIFIER,name,null,0);
            Function generated = new(token,expr.Params,expr.Body); 
            LoxFunction function = new (generated,_environment, FunctionType.FUNCTION);

            _environment.Define(name,function);
            return function;
        }



        public object? VisitCollectionExpr(Collection expr)
        {
            List<object?> collection = [];

            foreach (var element in expr.Elements)
            {
                var value = Evaluate(element);
                collection.Add(value);
            }

            return new LoxCollection(collection);
        }

        public object? VisitAssignExpr(Assign expr)
        {
            object? value = Evaluate(expr.Value);
            int? distance = _locals[expr];
            if (distance != null)
            {
                _environment.AssignAt(distance,expr.Name,value);
            }
            else
            {
                Globals.Assign(expr.Name, value);
            }

            return value;
        }

        private object? Evaluate(Expr? expr) => expr?.Accept(this);

        public object? VisitGetExpr(Get expr)
        {
            object? @object = Evaluate(expr.Object);

            if (@object is LoxInstance instance)
                return instance.Get(expr.Name);
            

            throw new RuntimeError(expr.Name, "только экземпляры могут использовать свойства");
        }

        public object? VisitSetExpr(Set expr)
        {
            object? @object = Evaluate(expr.Object);
            object? value = Evaluate(expr.Value);

            if (@object is LoxInstance instance)
            {
                instance.Set(expr.Name,value);
                return null;
            }


            throw new RuntimeError(expr.Name, "только экземпляры могут использовать свойства");
        }

        public object? VisitThisExpr(This expr) => LookUpVariable(expr.Keyword, expr);
    }
}
