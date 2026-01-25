using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.Types;

namespace TuchinC.Lexical
{
    internal class Scanner
    {
        private static readonly List<string> _types = [
                "bool",
                "char",
                "i8",
                "i16",
                "i32",
                "i64",
                "d16",
                "d32",
                "d64",
                "str",
            ];

        private static readonly Dictionary<string, TokenType> _keywords = new()
        {
            ["for"] = TokenType.FOR,
            ["loop"] = TokenType.LOOP,
            ["fun"] = TokenType.FUN,
            ["return"] = TokenType.RETURN,
            ["if"] = TokenType.IF,
            ["elif"] = TokenType.ELIF,
            ["else"] = TokenType.ELSE,
            ["switch"] = TokenType.SWITCH,
            ["case"] = TokenType.CASE,
            ["default"] = TokenType.DEFAULT,
            ["break"] = TokenType.BREAK,
            ["struct"] = TokenType.STRUCT,
            ["this"] = TokenType.THIS,
            ["let"] = TokenType.LET,
            ["true"] = TokenType.TRUE,
            ["false"] = TokenType.FALSE,
            ["nil"] = TokenType.NIL,
            ["use"] = TokenType.USE,
        };

        private readonly string source;
        private readonly List<Token> tokens = [];
        private int start = 0;
        private int current = 0;
        private int line = 1;
        internal Scanner(string source) 
        {
            this.source = source;
        }


        //Основной цикл
        internal List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }
            Token token = new(TokenType.EOF,"",null,line);
            tokens.Add(token);
            return tokens;

        }

        //Определяет завершение исходного кода
        private bool IsAtEnd() => current >= source.Length;


        //Основной метод обработки
        private void ScanToken()
        {
            char c = Advance();

            switch (c)
            {
                //Обработка одиночных лексем
                case '@': AddToken(TokenType.MACRO); break;
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '[': AddToken(TokenType.LEFT_BRACKET); break;
                case ']': AddToken(TokenType.RIGHT_BRACKET); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '$': AddToken(TokenType.CONCATE); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break;
                case '?': AddToken(TokenType.QUESTION); break;
                case ':': AddToken(TokenType.COLON); break;
                case '&': AddToken(Match('&') ?TokenType.AND:TokenType.AND_BIT); break;
                case '|': AddToken(Match('|') ?TokenType.VLINE_VLINE: TokenType.VLINE); break;
                case '^': AddToken(TokenType.XOR); break;
                case '~': AddToken(TokenType.NOT_BIT); break;
                case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL:TokenType.BANG); break;
                case '=':
                    
                    if (Match('='))
                        AddToken(TokenType.EQUAL_EQUAL);
                    else if (Match('>'))
                        AddToken(TokenType.ARROW);
                    else
                        AddToken(TokenType.EQUAL);

                    break;
                case '<': AddToken(Match('<')? TokenType.LEFT_OFFSET:Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(Match('>') ? TokenType.RIGHT_OFFSET : Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;



                //Обработка слеша и определение по максимальному поглащению
                //является ли лексема коментарием или делением
                case '/':
                    if (Match('/'))
                    {
                        while (PeekNext() != '\n' && !IsAtEnd()) 
                            Advance();
                    }
                    else
                    {
                        AddToken(TokenType.SLASH); 
                    }

                  break;

                //Обработка мусорных лексем
                case ' ':
                case '\r':
                case '\t':
                    break;

                case '\n':
                    line++;
                    break;


                //Обработка строкового литерала
                case '"': String(); break;


                default:

                    //Обработка числового литерала
                    if (IsDigit(c))
                        Number();
                    else if (IsAlpha(c))
                        Identifier();
                    else
                        Tuchin.Error(line, $"Неизвестный символ '{c}'");

                    break;
            }


        }


        //Переход к следующему символу
        private char Advance() 
        {
            current++;
            return source[current - 1];
        }


        //Возвращает текущий символ исходного кода либо конец файла
        private char Peek() 
        {
            if (IsAtEnd()) return '\0';

            return source[current];
        }


        //Возвращает следующий символ исходного кода либо конец файла
        private char PeekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }


        //Определение идентификатора
        private void Identifier()
        {
            while (IsAlphaNumberic(Peek())) Advance();

            string text = source[start..current];

            //Если текущий текст не является зарезервированным словом
            //или примитивным типом то он является идентификатором
            if (!_keywords.TryGetValue(text, out TokenType type))
                type = _types.Contains(text) ? TokenType.PRIMITIVE_TYPE : TokenType.IDENTIFIER;

            AddToken(type);
        }


        
        

        //Определяет является ли символ буквой алфавита
        private static bool IsAlpha(char c) => c >= 'a' && c<= 'z' 
            || c>= 'A' && c<= 'Z' 
            || c == '_';


        //Определяет является ли символ числом или буквой
        private static bool IsAlphaNumberic(char c) => IsAlpha(c) || IsDigit(c);



        //Определяет является ли символ числом
        private static bool IsDigit(char c) => c >= '0' && c <= '9';


        //Обработка числового литерала
        private void Number()
        {
            while (IsDigit(Peek())) Advance();

            if(Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance();

                while (IsDigit(Peek())) Advance();

            }

            string literal = source[start..current];
            literal = literal.IndexOf('.') >0 ?literal.Replace('.',','):literal;
            AddToken(TokenType.NUMBER,double.Parse(literal));

        }


        //Обработка строкового литерала
        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') line++;
               
                Advance();

            }

            if (IsAtEnd())
            {
                Tuchin.Error(line,"Unterminated string.");
                return;
            }

            Advance();

            string value = source[(start+1)..(current-1)];
            AddToken(TokenType.STRING,value);

        }


        //Добавление токена через тип игнорируя литеральное значение
        private void AddToken(TokenType type) => AddToken(type,null);


        //Добавление токена через тип и литеральное значение
        private void AddToken(TokenType type, object? literal) 
        {
            string text = source[start..current];
            Token token = new(type,text,literal,line);
            tokens.Add(token);
        }


        //Проверка на совместимость следующего не равного исходному
        //символа с expected
        private bool Match(char expected) 
        {
            if (IsAtEnd()) return false;
            if (source[current] != expected) return false;

            current++;
            return true;

        }



    }
}
