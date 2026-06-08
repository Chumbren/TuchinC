using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Expressions.Calles;
using TuchinC.AST.Nodes.Expressions.Visitors;
using TuchinC.AST.Semantic.Types;

namespace TuchinC.GenerateByte
{
    public partial class Generator : IVisitor<object?>
    {
        public void Generate(Expr? expr) => expr?.Accept(this);

        public object? VisitArrowFunction(ArrowFunction expr)
        {
            return null;
        }

        public object? VisitAssignExpr(Assign expr)
        {
            Assign(expr.Name.Lexeme, expr.Value);
            return null;
        }

        private void Assign(string name, Expr? value)
        {
            EmitByte(ByteCode.Assign);
            EmitString(name);
            Generate(value);
        }

        public object? VisitBinaryExpr(Binary expr)
        {
            EmitByte(ByteCode.Binnary);

            switch (expr.Operator.Type)
            {
                case TokenType.PLUS:
                    EmitByte(ByteCode.Add);
                    break;
                case TokenType.MINUS:
                    EmitByte(ByteCode.Sub);
                    break;
                case TokenType.SLASH:
                    EmitByte(ByteCode.Devide);
                    break;
                case TokenType.STAR:
                    EmitByte(ByteCode.Multiply);
                    break;
                case TokenType.XOR:
                    EmitByte(ByteCode.XOR);
                    break;
                case TokenType.AND_BIT:
                    EmitByte(ByteCode.BitMultiply);
                    break;
                case TokenType.VLINE:
                    EmitByte(ByteCode.BitAdd);
                    break;
                case TokenType.LEFT_OFFSET:
                    EmitByte(ByteCode.BitLeftOffset);
                    break;
                case TokenType.RIGHT_OFFSET:
                    EmitByte(ByteCode.BitRightOffset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expr), expr, "Оператор не определен");
            }

            Generate(expr.Left);
            Generate(expr.Right);
            return null;
        }

        private void EmitLogicalExpr(Logical binary)
        {
            EmitByte(ByteCode.Logical);

            switch (binary.Operator.Type)
            {
                case TokenType.EQUAL_EQUAL:
                    EmitByte(ByteCode.Equil);
                    break;
                case TokenType.BANG_EQUAL:
                    EmitByte(ByteCode.BangEquil);
                    break;
                case TokenType.LESS:
                    EmitByte(ByteCode.Less);
                    break;
                case TokenType.LESS_EQUAL:
                    EmitByte(ByteCode.LessEquil);
                    break;
                case TokenType.GREATER:
                    EmitByte(ByteCode.Greater);
                    break;
                case TokenType.GREATER_EQUAL:
                    EmitByte(ByteCode.GreaterEquil);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(binary), binary, "Оператор не определен");
            }

            Generate(binary.Left);
            Generate(binary.Right);
        }

        public object? VisitCallExpr(Call expr)
        {
            if (expr is FunctionCall fcall)
            {
                EmitFunctionCallExpr(fcall);
            }
            else
            {
                throw new NotImplementedException();
            }

            return null;
        }

        private void EmitFunctionCallExpr(in FunctionCall call)
        {
            EmitByte(ByteCode.Call);
            // Ожидаем, что call.Calle - это Variable с именем функции
            if (call.Calle is Variable varExpr)
            {
                EmitString(varExpr.Name.Lexeme);
            }
            else
            {
                // Если выражение, генерируем его
                Generate(call.Calle);
            }
            EmitArgumentsFunctionCall(call.Arguments);
        }

        private void EmitArgumentsFunctionCall(List<Expr> args)
        {
            EmitInt32(args.Count);
            foreach (var arg in args)
                Generate(arg);
        }

        public object? VisitCollectionExpr(Collection expr)
        {
            return null;
        }

        public object? VisitGetExpr(Get expr)
        {
            return null;
        }

        public object? VisitGroupingExpr(Grouping expr)
        {
            Generate(expr.Expression);
            return null;
        }

        public object? VisitLiteralExpr(Literal expr)
        {
            EmitByte(ByteCode.Literal);
            EmitLiteralType(expr);
            return null;
        }

        private void EmitLiteralType(Literal expr)
        {
            EmitByte(expr.Type.Type);

            if (expr.Value == null)
            {
                EmitByte((byte)TypeValue.Nil);
                return;
            }
            else if (expr.Type.IsPrimitive())
            {
                EmitType(expr.Type, expr.Value);
                return;
            }

            throw new ArgumentOutOfRangeException(nameof(expr), "Тип значения литерала не поддерживается генерации в ветке VisitLiteral");
        }

        private static byte[] GetDecimalBytes(decimal value)
        {
            byte[] bytes = [];
            using (MemoryStream ms = new())
            {
                using BinaryWriter bw = new(ms);
                bw.Write(value);
                bytes = ms.ToArray();
            }
            return bytes;
        }

        public object? VisitLogicalExpr(Logical expr)
        {
            EmitLogicalExpr(expr);
            return null;
        }

        public object? VisitSetExpr(Set expr)
        {
            return null;
        }

        public object? VisitTernaryExpr(Ternary expr)
        {
            EmitByte(ByteCode.Ternary);
            EmitByte(ByteCode.Condition);
            Generate(expr.If);
            EmitByte(ByteCode.JmpIf);
            EmitWaitInt32(); // Смещение для JmpIf
            Generate(expr.That);
            Generate(expr.Else);
            return null;
        }

        public object? VisitThisExpr(This expr)
        {
            return null;
        }

        public object? VisitUnaryExpr(Unary expr)
        {
            EmitByte(ByteCode.Unary);
            switch (expr.Operator.Type)
            {
                case TokenType.NOT_BIT:
                    EmitByte(ByteCode.Not);
                    break;
                case TokenType.BANG:
                    EmitByte(ByteCode.Bang);
                    break;
                case TokenType.PLUS:
                    EmitByte(ByteCode.Add); // Унарный плюс ничего не делает
                    break;
                case TokenType.MINUS:
                    EmitByte(ByteCode.Sub); // Унарный минус
                    break;
                default:
                    break;
            }
            Generate(expr.Right);
            return null;
        }

        public object? VisitVariableExpr(Variable expr)
        {
            EmitByte(ByteCode.PeekCopy);
            EmitString(expr.Name.Lexeme);
            return null;
        }
    }
}