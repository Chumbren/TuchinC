using System;
using System.Collections.Generic;
using System.Text;
using TuchinC.AST.Semantic.Types;

namespace TuchinC.GenerateByte.Disassemble
{
    public class Disassembler
    {
        private readonly byte[] _bytes;
        private int _pos;
        private readonly StringBuilder _output = new();
        private int _indent = 0;

        public Disassembler(byte[] bytes)
        {
            _bytes = bytes;
        }

        private void WriteLine(string line)
        {
            _output.Append(' ', _indent * 2);
            _output.AppendLine(line);
        }

        private void Write(string text)
        {
            _output.Append(' ', _indent * 2);
            _output.Append(text);
        }

        private string ReadStringRaw()
        {
            if (_pos + 4 > _bytes.Length)
                return "";

            int len = BitConverter.ToInt32(_bytes, _pos);
            _pos += 4;

            if (len <= 0 || len > 10000 || _pos + len > _bytes.Length)
                return "";

            string value = Encoding.UTF8.GetString(_bytes, _pos, len);
            _pos += len;

            return value;
        }

        private int ReadInt32()
        {
            if (_pos + 4 > _bytes.Length)
                return 0;

            int value = BitConverter.ToInt32(_bytes, _pos);
            _pos += 4;
            return value;
        }

        private void ReadLiteral()
        {
            if (_pos >= _bytes.Length)
            {
                WriteLine("Literal (missing type)");
                return;
            }

            TypeValue litType = (TypeValue)_bytes[_pos];
            _pos++;

            Write($"Literal {litType}");

            switch (litType)
            {
                case TypeValue.Int8:
                    if (_pos >= _bytes.Length) break;
                    sbyte i8 = (sbyte)_bytes[_pos];
                    WriteLine($" {i8}");
                    _pos++;
                    break;

                case TypeValue.Int16:
                    if (_pos + 2 > _bytes.Length) break;
                    short i16 = BitConverter.ToInt16(_bytes, _pos);
                    WriteLine($" {i16}");
                    _pos += 2;
                    break;

                case TypeValue.Int32:
                    if (_pos + 4 > _bytes.Length) break;
                    int i32 = BitConverter.ToInt32(_bytes, _pos);
                    WriteLine($" {i32}");
                    _pos += 4;
                    break;

                case TypeValue.Int64:
                    if (_pos + 8 > _bytes.Length) break;
                    long i64 = BitConverter.ToInt64(_bytes, _pos);
                    WriteLine($" {i64}");
                    _pos += 8;
                    break;

                case TypeValue.Double32:
                    if (_pos + 4 > _bytes.Length) break;
                    float f32 = BitConverter.ToSingle(_bytes, _pos);
                    WriteLine($" {f32}");
                    _pos += 4;
                    break;

                case TypeValue.Double64:
                    if (_pos + 8 > _bytes.Length) break;
                    double f64 = BitConverter.ToDouble(_bytes, _pos);
                    WriteLine($" {f64}");
                    _pos += 8;
                    break;

                case TypeValue.String:
                    if (_pos + 4 > _bytes.Length) break;
                    int len = ReadInt32();
                    if (len < 0 || len > 10000 || _pos + len > _bytes.Length) break;
                    string str = Encoding.UTF8.GetString(_bytes, _pos, len);
                    WriteLine($" \"{str}\"");
                    _pos += len;
                    break;

                case TypeValue.Char:
                    if (_pos >= _bytes.Length) break;
                    char ch = (char)_bytes[_pos];
                    WriteLine($" '{ch}'");
                    _pos++;
                    break;

                case TypeValue.Boolean:
                    if (_pos >= _bytes.Length) break;
                    bool b = _bytes[_pos] != 0;
                    WriteLine($" {b}");
                    _pos++;
                    break;

                case TypeValue.Nil:
                    WriteLine(" nil");
                    break;

                default:
                    WriteLine($" <unknown type: {litType}>");
                    break;
            }
        }

        private void DisassembleIf()
        {
            WriteLine("If");
            _indent++;

            // Читаем размер всего блока If
            if (_pos + 4 > _bytes.Length)
            {
                WriteLine("; ERROR: Not enough bytes for block size");
                _indent--;
                return;
            }

            int totalBlockSize = ReadInt32();
            int blockStart = _pos;
            WriteLine($"; total if block size: {totalBlockSize}");

            // Читаем if ветку
            WriteLine($"; if branch:");
            _indent++;
            DisassembleBranch();
            _indent--;

            // Читаем количество elif веток
            if (_pos + 4 > _bytes.Length)
            {
                WriteLine("; ERROR: Not enough bytes for elif count");
                _indent--;
                return;
            }

            int elifCount = ReadInt32();
            WriteLine($"; elif count: {elifCount}");

            // Читаем каждую elif ветку
            for (int i = 0; i < elifCount; i++)
            {
                WriteLine($"; elif {i + 1} branch:");
                _indent++;
                DisassembleBranch();
                _indent--;
            }

            // Читаем флаг наличия else
            if (_pos >= _bytes.Length)
            {
                WriteLine("; ERROR: Not enough bytes for else flag");
                _indent--;
                return;
            }

            bool hasElse = _bytes[_pos] != 0;
            _pos++;
            WriteLine($"; has else: {hasElse}");

            // Если есть else, читаем его тело напрямую (без JmpIf)
            if (hasElse)
            {
                WriteLine($"; else branch:");
                _indent++;

                // Сохраняем позицию перед else
                int elseStart = _pos;

                // Дизассемблируем тело else
                // Здесь нужно дизассемблировать инструкции до конца блока If
                while (_pos < blockStart + totalBlockSize && _pos < _bytes.Length)
                {
                    if (_pos >= blockStart + totalBlockSize) break;

                    Write($"0x{_pos:X4}: ");
                    byte op = _bytes[_pos];
                    _pos++;

                    // Временно выходим из рекурсии - дизассемблируем текущую инструкцию
                    // Для простоты, пропускаем тело else
                    WriteLine($"; ... (else body byte 0x{op:X2})");
                }

                _indent--;
            }

            // Проверяем, что мы прочитали правильное количество байт
            if (_pos != blockStart + totalBlockSize)
            {
                WriteLine($"; WARNING: Read {_pos - blockStart} bytes, expected {totalBlockSize}");
                _pos = blockStart + totalBlockSize;
            }

            _indent--;
        }

        private void DisassembleBranch()
        {
            WriteLine($"; condition:");
            _indent++;

            // Дизассемблируем выражение условия до JmpIf
            int startPos = _pos;
            bool foundJmpIf = false;

            while (_pos < _bytes.Length && !foundJmpIf)
            {
                // Временно сохраняем позицию для проверки JmpIf
                if (_bytes[_pos] == (byte)ByteCode.JmpIf)
                {
                    foundJmpIf = true;
                    break;
                }

                // Дизассемблируем текущую инструкцию (часть условия)
                Write($"0x{_pos:X4}: ");
                byte op = _bytes[_pos];
                _pos++;

                // Временный мини-дизассемблер для инструкций условия
                switch ((ByteCode)op)
                {
                    case ByteCode.PeekCopy:
                        string name = ReadStringRaw();
                        WriteLine($"PeekCopy {name}");
                        break;
                    case ByteCode.Literal:
                        // Нужно вернуть байт типа обратно для ReadLiteral
                        _pos--;
                        ReadLiteral();
                        break;
                    case ByteCode.Binnary:
                        if (_pos < _bytes.Length)
                        {
                            byte binOp = _bytes[_pos];
                            _pos++;
                            WriteLine($"Binnary {(ByteCode)binOp}");
                        }
                        else
                            WriteLine("Binnary (missing operator)");
                        break;
                    case ByteCode.Logical:
                        if (_pos < _bytes.Length)
                        {
                            byte logOp = _bytes[_pos];
                            _pos++;
                            WriteLine($"Logical {(ByteCode)logOp}");
                        }
                        else
                            WriteLine("Logical (missing operator)");
                        break;
                    default:
                        WriteLine($"??? (0x{op:X2})");
                        break;
                }
            }

            _indent--;

            if (!foundJmpIf)
            {
                WriteLine($"; ERROR: JmpIf not found in branch");
                return;
            }

            // Читаем JmpIf
            Write($"0x{_pos:X4}: ");
            _pos++;

            // Читаем смещение
            if (_pos + 4 > _bytes.Length)
            {
                WriteLine($"JmpIf (incomplete offset)");
                return;
            }

            int offset = ReadInt32();
            int jumpTarget = _pos + offset;
            WriteLine($"JmpIf +{offset} (to 0x{jumpTarget:X4})");

            // Читаем тело ветки
            int bodyStart = _pos;
            int bodyEnd = bodyStart + offset;

            WriteLine($"; branch body ({offset} bytes):");
            _indent++;

            // Дизассемблируем тело ветки рекурсивно
            int savedPos = _pos;
            _pos = bodyStart;

            while (_pos < bodyEnd && _pos < _bytes.Length)
            {
                if (_pos >= bodyEnd) break;

                Write($"0x{_pos:X4}: ");
                byte op = _bytes[_pos];
                _pos++;

                // Основной дизассемблинг инструкции
                switch ((ByteCode)op)
                {
                    case ByteCode.Push:
                        if (_pos < _bytes.Length && _bytes[_pos] == 0x30)
                        {
                            WriteLine("Push Stack");
                            _pos++;
                        }
                        else
                        {
                            string name = ReadStringRaw();
                            if (!string.IsNullOrEmpty(name) && _pos < _bytes.Length)
                            {
                                TypeValue type = (TypeValue)_bytes[_pos];
                                _pos++;
                                WriteLine($"Push {name} : {type}");
                            }
                            else
                                WriteLine($"Push (error)");
                        }
                        break;
                    case ByteCode.Pop:
                        if (_pos < _bytes.Length && _bytes[_pos] == 0x30)
                        {
                            WriteLine("Pop Stack");
                            _pos++;
                        }
                        else
                        {
                            string name = ReadStringRaw();
                            WriteLine($"Pop {name}");
                        }
                        break;
                    case ByteCode.Assign:
                        string assignName = ReadStringRaw();
                        WriteLine($"Assign {assignName}");
                        break;
                    case ByteCode.Literal:
                        _pos--;
                        ReadLiteral();
                        break;
                    case ByteCode.Add:
                        WriteLine("Add");
                        break;
                    case ByteCode.Stack:
                        WriteLine("Stack");
                        break;
                    case ByteCode.Call:
                        string funcName = ReadStringRaw();
                        WriteLine($"Call {funcName}");
                        break;
                    default:
                        WriteLine($"UNKNOWN_0x{op:X2}");
                        break;
                }
            }

            _indent--;
            _pos = bodyEnd;
        }

        private void DisassembleLoop()
        {
            WriteLine("Loop");
            _indent++;

            if (_pos + 4 <= _bytes.Length)
            {
                int loopSize = ReadInt32();
                WriteLine($"; loop size: {loopSize}");

                if (loopSize > 0 && _pos + loopSize <= _bytes.Length)
                {
                    WriteLine($"; skipping loop body ({loopSize} bytes)");
                    _pos += loopSize;
                }
                else
                {
                    WriteLine($"; WARNING: Invalid loop size {loopSize}");
                }
            }
            else
            {
                WriteLine($"; ERROR: Not enough bytes for loop size");
            }

            _indent--;
        }

        public string Disassemble()
        {
            // Проверяем магическое число "tuchin"
            if (_bytes.Length < 6)
            {
                return "Error: File too small";
            }

            string magic = Encoding.UTF8.GetString(_bytes, 0, 6);
            if (magic != "tuchin")
            {
                WriteLine($"; Warning: Invalid magic number '{magic}', expected 'tuchin'");
                _pos = 0;
            }
            else
            {
                WriteLine("; Magic: tuchin");
                _pos = 6;
            }

            WriteLine("");
            WriteLine("; === Bytecode Disassembly ===");
            WriteLine("");

            while (_pos < _bytes.Length)
            {
                // Проверка на выход за границы
                if (_pos >= _bytes.Length)
                    break;

                // Показываем текущую позицию
                Write($"0x{_pos:X4}: ");

                byte op = _bytes[_pos];
                _pos++;

                switch ((ByteCode)op)
                {
                    case ByteCode.EOF:
                        WriteLine("EOF");
                        return _output.ToString();

                    case ByteCode.Push:
                        if (_pos < _bytes.Length && _bytes[_pos] == 0x30) // Stack
                        {
                            WriteLine("Push Stack");
                            _pos++;
                        }
                        else
                        {
                            string name = ReadStringRaw();
                            if (!string.IsNullOrEmpty(name) && _pos < _bytes.Length)
                            {
                                TypeValue type = (TypeValue)_bytes[_pos];
                                _pos++;
                                WriteLine($"Push {name} : {type}");
                            }
                            else
                            {
                                WriteLine($"Push (error at 0x{_pos:X4})");
                            }
                        }
                        break;

                    case ByteCode.Pop:
                        if (_pos < _bytes.Length && _bytes[_pos] == 0x30)
                        {
                            WriteLine("Pop Stack");
                            _pos++;
                        }
                        else
                        {
                            string name = ReadStringRaw();
                            if (!string.IsNullOrEmpty(name))
                            {
                                WriteLine($"Pop {name}");
                            }
                            else
                            {
                                WriteLine($"Pop (error at 0x{_pos:X4})");
                            }
                        }
                        break;

                    case ByteCode.Assign:
                        {
                            string name = ReadStringRaw();
                            if (!string.IsNullOrEmpty(name))
                            {
                                WriteLine($"Assign {name}");
                            }
                            else
                            {
                                WriteLine($"Assign (error at 0x{_pos:X4})");
                            }
                        }
                        break;

                    case ByteCode.PeekCopy:
                        {
                            string name = ReadStringRaw();
                            if (!string.IsNullOrEmpty(name))
                            {
                                WriteLine($"PeekCopy {name}");
                            }
                            else
                            {
                                WriteLine($"PeekCopy (error at 0x{_pos:X4})");
                            }
                        }
                        break;

                    case ByteCode.PeekRef:
                        {
                            string name = ReadStringRaw();
                            if (!string.IsNullOrEmpty(name))
                            {
                                WriteLine($"PeekRef {name}");
                            }
                            else
                            {
                                WriteLine($"PeekRef (error at 0x{_pos:X4})");
                            }
                        }
                        break;

                    case ByteCode.Literal:
                        ReadLiteral();
                        break;

                    case ByteCode.Binnary:
                        if (_pos < _bytes.Length)
                        {
                            byte binOp = _bytes[_pos];
                            _pos++;
                            WriteLine($"Binnary {(ByteCode)binOp}");
                        }
                        else
                        {
                            WriteLine("Binnary (missing operator)");
                        }
                        break;

                    case ByteCode.Logical:
                        if (_pos < _bytes.Length)
                        {
                            byte logOp = _bytes[_pos];
                            _pos++;
                            WriteLine($"Logical {(ByteCode)logOp}");
                        }
                        else
                        {
                            WriteLine("Logical (missing operator)");
                        }
                        break;

                    case ByteCode.Unary:
                        if (_pos < _bytes.Length)
                        {
                            byte unaryOp = _bytes[_pos];
                            _pos++;
                            WriteLine($"Unary {(ByteCode)unaryOp}");
                        }
                        else
                        {
                            WriteLine("Unary (missing operator)");
                        }
                        break;

                    case ByteCode.Ternary:
                        WriteLine("Ternary");
                        break;

                    case ByteCode.Condition:
                        WriteLine("Condition");
                        break;

                    case ByteCode.Call:
                        {
                            string funcName = ReadStringRaw();
                            if (!string.IsNullOrEmpty(funcName))
                            {
                                WriteLine($"Call {funcName}");
                                if (_pos + 4 <= _bytes.Length)
                                {
                                    int argCount = ReadInt32();
                                    WriteLine($"  ; args count: {argCount}");
                                }
                            }
                            else
                            {
                                WriteLine($"Call (error at 0x{_pos:X4})");
                            }
                        }
                        break;

                    case ByteCode.Jmp:
                        if (_pos + 4 <= _bytes.Length)
                        {
                            int offset = ReadInt32();
                            WriteLine($"Jmp +{offset} (to 0x{_pos + offset:X4})");
                        }
                        else
                        {
                            WriteLine("Jmp (incomplete offset)");
                        }
                        break;

                    case ByteCode.JmpIf:
                        if (_pos + 4 <= _bytes.Length)
                        {
                            int offset = ReadInt32();
                            WriteLine($"JmpIf +{offset} (to 0x{_pos + offset:X4})");
                        }
                        else
                        {
                            WriteLine("JmpIf (incomplete offset)");
                        }
                        break;

                    case ByteCode.If:
                        DisassembleIf();
                        break;

                    case ByteCode.Loop:
                        DisassembleLoop();
                        break;

                    case ByteCode.Stack:
                        WriteLine("Stack");
                        break;

                    // Арифметические операции
                    case ByteCode.Add:
                        WriteLine("Add");
                        break;
                    case ByteCode.Sub:
                        WriteLine("Sub");
                        break;
                    case ByteCode.Multiply:
                        WriteLine("Multiply");
                        break;
                    case ByteCode.Devide:
                        WriteLine("Divide");
                        break;
                    case ByteCode.Bang:
                        WriteLine("Bang");
                        break;
                    case ByteCode.BangEquil:
                        WriteLine("BangEquil");
                        break;
                    case ByteCode.Equil:
                        WriteLine("Equil");
                        break;
                    case ByteCode.Greater:
                        WriteLine("Greater");
                        break;
                    case ByteCode.GreaterEquil:
                        WriteLine("GreaterEquil");
                        break;
                    case ByteCode.Less:
                        WriteLine("Less");
                        break;
                    case ByteCode.LessEquil:
                        WriteLine("LessEquil");
                        break;
                    case ByteCode.Not:
                        WriteLine("Not");
                        break;
                    case ByteCode.XOR:
                        WriteLine("XOR");
                        break;
                    case ByteCode.BitAdd:
                        WriteLine("BitAdd");
                        break;
                    case ByteCode.BitMultiply:
                        WriteLine("BitMultiply");
                        break;
                    case ByteCode.BitLeftOffset:
                        WriteLine("BitLeftOffset");
                        break;
                    case ByteCode.BitRightOffset:
                        WriteLine("BitRightOffset");
                        break;

                    default:
                        WriteLine($"UNKNOWN_0x{op:X2} (at 0x{_pos - 1:X4})");
                        break;
                }
            }

            WriteLine("\n; End of disassembly");
            return _output.ToString();
        }
    }
}