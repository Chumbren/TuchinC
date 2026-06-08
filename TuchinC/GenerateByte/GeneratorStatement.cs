using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements;
using TuchinC.AST.Nodes.Statements.Visitors;

namespace TuchinC.GenerateByte
{
    public partial class Generator : IVisitor
    {
        public void Generate(Stmt? stmt) => stmt?.Accept(this);

        public void VisitBlockStmt(Block stmt) => Block(stmt);

        private void Block(Block block)
        {
            Push();
            foreach (var stmt in block.Statements)
                Generate(stmt);
            Pop();
        }

        public void VisitClassStmt(Struct stmt)
        {
            throw new NotImplementedException();
        }

        public void VisitExpressionStmt(Expression stmt) => Generate(stmt.Value);

        public void VisitFunctionStmt(Function stmt)
        {
            throw new NotImplementedException("Functions are not implemented yet");
        }

        public void EmitFunctionArgumentsStmt(List<Param> args)
        {
            EmitInt32(args.Count);
            foreach (var arg in args)
            {
                EmitString(arg.Name.Lexeme);
            }
        }

        private int ReserveInt32()
        {
            int position = _bytes.Count;
            _bytes.Add(0);
            _bytes.Add(0);
            _bytes.Add(0);
            _bytes.Add(0);
            return position;
        }

        private void WriteInt32At(int position, int value)
        {
            if (position + 4 > _bytes.Count) return;
            byte[] bytes = BitConverter.GetBytes(value);
            for (int i = 0; i < 4; i++)
            {
                _bytes[position + i] = bytes[i];
            }
        }

        public void VisitIfStmt(If stmt)
        {
            EmitByte(ByteCode.If);

            // Резервируем место для общего размера блока If
            int blockSizePos = ReserveInt32();
            int ifStart = _bytes.Count;

            // Генерируем основную if ветку
            GenerateBranch(stmt.Condition, stmt.ThenBranch);

            // Генерируем elif ветки
            EmitInt32(stmt.ElifBranches.Count);
            foreach (var elif in stmt.ElifBranches)
            {
                GenerateBranch(elif.Condition, elif.ThenBranch);
            }

            // Генерируем else ветку если есть
            bool hasElse = stmt.ElseBranch != null;
            EmitByte(hasElse ? (byte)1 : (byte)0);

            if (hasElse)
            {
                // Else ветка не нуждается в JmpIf
                Generate(stmt.ElseBranch);
            }

            // Вычисляем и записываем общий размер блока If
            int ifBlockSize = _bytes.Count - ifStart;
            WriteInt32At(blockSizePos, ifBlockSize);
        }

        private void GenerateBranch(Expr condition, Stmt body)
        {
            // Генерируем условие
            Generate(condition);

            // JmpIf прыгает через тело если условие ложно
            EmitByte(ByteCode.JmpIf);
            int jmpOffsetPos = ReserveInt32();

            int start = _bytes.Count;

            // Генерируем тело ветки
            Generate(body);

            int bodySize = _bytes.Count - start;

            // Заполняем смещение для JmpIf
            WriteInt32At(jmpOffsetPos, bodySize);
        }

        public void VisitImportStmt(Use stmt)
        {
            throw new NotImplementedException();
        }

        public void VisitLetStmt(Let stmt)
        {
            Push(stmt.Name.Lexeme, new ValueType(stmt.Type));
            Assign(stmt.Name.Lexeme, stmt.Initializer);
        }

        public void VisitLoopStmt(Loop stmt)
        {
            throw new NotImplementedException();
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
            EmitByte(ByteCode.Call);
            EmitString("ToString");
            Generate(stmt.Expression);
        }
    }
}