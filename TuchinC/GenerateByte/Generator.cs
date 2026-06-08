using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Xml.Linq;
using TuchinC.AST.Nodes.Statements;
using TuchinC.AST.Semantic.Types;
using TuchinC.CodeAnalize.Emiters;

namespace TuchinC.GenerateByte
{
    internal enum ByteCode : byte
    {
        //Конец файла
        EOF = 0x00,

        //Общие операции
        Add = 0x01,
        Sub = 0x02,
        Devide = 0x03,
        Multiply = 0x04,

        //Логические операции
        Bang = 0x10,
        BangEquil = 0x11,
        Equil = 0x12,
        Greater = 0x13,
        GreaterEquil = 0x14,
        Less = 0x015,
        LessEquil = 0x16,

        //Побитовые операции
        Not = 0x20,
        XOR = 0x21,
        BitAdd = 0x22,
        BitMultiply = 0x23,
        BitLeftOffset = 0x24,
        BitRightOffset = 0x25,


        Stack = 0x30,
        Literal = 0x31,
        Binnary = 0x33,
        Logical = 0x34,
        Unary = 0x35,
        Ternary = 0x36,
        Call = 0x37,

        Jmp = 0x40,
        JmpIf = 0x41,
        Push = 0x44,
        Pop = 0x45,
        Assign = 0x46,
        PeekCopy = 0x47,
        PeekRef = 0x48,
        Condition = 0x49,

        //Упраавление потоком реального времени
        If = 0x50,
        Loop = 0x51,
    }

    public partial class Generator(string project, List<Stmt?> ast): EmitWaiter<byte>
    {
        private readonly List<byte> _bytes = [];
        private readonly List<Stmt?> _ast = ast;
        private readonly Stack<Dictionary<string, ValueType>> _scopes = [];
        private readonly Stack<(int, int)> _waits = [];
        public IReadOnlyList<byte> Bytes => _bytes.AsReadOnly();
        public readonly string Project = project;

        public List<byte> Generate()
        {
            //Флаг показывающий что байт файл написан на tuchin
            var tuchin = Encoding.UTF8.GetBytes("tuchin");
            _bytes.AddRange(tuchin);

            Push();
            foreach (var item in _ast)
                Generate(item);

            Pop();
            EmitByte(ByteCode.EOF);
            return _bytes;
        }


        private void EmitByte(TypeValue type) => _bytes.Add((byte)type);
        
        private void EmitByte(ByteCode @byte) => _bytes.Add((byte)@byte);
        private void EmitByte(params ByteCode[] bytes)
        {
            foreach (var @byte in bytes)
                _bytes.Add((byte)@byte);
        }
        private void EmitByte(byte @byte) => _bytes.Add(@byte);
        
        private void EmitBytes(params byte[] @bytes) => _bytes.AddRange(@bytes);

        private void EmitString(string @string)
        {
            byte[] length = BitConverter.GetBytes(@string.Length);
            EmitBytes(length);
            EmitBytes(Encoding.UTF8.GetBytes(@string));
        }

        // Добавьте метод для эмиссии литеральной строки
        private void EmitLiteralString(string @string)
        {
            EmitByte(TypeValue.String);
            byte[] length = BitConverter.GetBytes(@string.Length);
            EmitBytes(length);
            EmitBytes(Encoding.UTF8.GetBytes(@string));
        }

        private void EmitType(ValueType type, object? value)
        {
            if (value == null)
            {
                EmitByte(TypeValue.Nil);
                return;
            }

            switch (type.Type)
            {
                case TypeValue.Char:
                    EmitByte((byte)(char)value);
                    break;
                case TypeValue.Boolean:
                    EmitByte((bool)value ? (byte)1 : (byte)0);
                    break;
                case TypeValue.Int8:
                    EmitByte((byte)value);
                    break;
                case TypeValue.Int16:
                    EmitBytes(BitConverter.GetBytes((short)value));
                    break;
                case TypeValue.Int32:
                    EmitBytes(BitConverter.GetBytes((int)value));
                    break;
                case TypeValue.Int64:
                    EmitBytes(BitConverter.GetBytes((long)value));
                    break;
                case TypeValue.Double32:
                    EmitBytes(BitConverter.GetBytes((float)value));
                    break;
                case TypeValue.Double64:
                    EmitBytes(BitConverter.GetBytes((double)value));
                    break;
                case TypeValue.Double128:
                    EmitBytes(GetDecimalBytes((decimal)value));
                    break;
                case TypeValue.String:
                case TypeValue.Identifier:
                    EmitString((string)value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), "Примитивный тип литерала не определен");
            }
        }

        private void EmitInt32(int value) => EmitBytes(BitConverter.GetBytes(value));

        private void EmitWaitInt32() => EmitWaitRange(4);

        private void EmitQuitInt32(int value)
        {
            var @bytes = BitConverter.GetBytes(value);
            EmitQuitRange(@bytes);
        }
        private void EmitJump()
        {
            EmitByte(ByteCode.Jmp);
            EmitWaitInt32();
        }


        private int CountBytes() =>_bytes.Count;

        private void Push()
        {
            EmitByte(ByteCode.Push);
            EmitByte(ByteCode.Stack);
            _scopes.Push([]);
        }

        private void Push(string name, ValueType type)
        {
            EmitByte(ByteCode.Push);
            EmitString($"{name}");
            EmitByte(type.Type);  

            var scope = _scopes.Peek();
            scope.Add(name, type);
        }

        private void Pop()
        {
            EmitByte(ByteCode.Pop);
            EmitByte(ByteCode.Stack);
            _scopes.Pop();
        }
        
        private void Pop(string name)
        {
            EmitByte(ByteCode.Pop);
            EmitString($"{name}");

            var scope = _scopes.Peek();
            scope.Remove(name);
        }

        private void ClearScope()
        {
            if (_scopes.Count == 0)
                return;

            var scope = _scopes.Peek();
            foreach (var pair in scope)
                Pop(pair.Key);
            
            Pop();
        }


        private bool ExistNameInScope(string name)
        {
            foreach (var scope in _scopes)
            {
                if (scope.TryGetValue(name, out _))
                    return true;
            }

            return false;
        }
        private void Assign(string name, ValueType type)
        {
            var scope = _scopes.Peek();
            scope[name] = type;
        }

    }
}
