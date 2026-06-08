using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuchinC.AST.Lexical
{
    public enum TokenType
    {
        //Одиночные или парные операторы
        LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
        LEFT_BRACKET, RIGHT_BRACKET, LEFT_OFFSET, RIGHT_OFFSET,
        COMMA, DOT, MINUS, PLUS, CONCATE,
        SEMICOLON, SLASH, STAR, QUESTION,
        COLON, AND_BIT, 
        XOR, NOT_BIT, ARROW, ARROW_OPERATOR, VLINE, VLINE_VLINE,
        MACRO,


        //Бинарные логические операторы    
        AND,
        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        //Литералы
        IDENTIFIER, LITERAL,

        //Ключевые слова
        STRUCT, IF, ELIF, ELSE, SWITCH, CASE, DEFAULT,
        BREAK , FALSE, FUN, FOR, IN ,  NIL, RETURN,
        THIS,  TRUE, LET, LOOP, USE, PRINT,

        //Модификаторы
        PUB, PRIVATE, EXTERN,

        //Конец файла
        EOF



    }

    public class Token(TokenType type,string lexeme,object? literal,int line)
    {
        public readonly TokenType Type = type;
        public readonly string Lexeme = lexeme;
        public readonly object? Literal = literal;
        public readonly int Line = line;

        public Token(TokenType type) : this(type,"",null,-1)
        {}
        public Token(TokenType type, string literal) : this(type,literal,null,-1)
        {}

        public override string ToString() => $"[{Line}]([ {Type} ]; [ {Lexeme} ]  [ {Literal ?? "Not literal"} ] )";

    }
}
