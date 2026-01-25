using TuchinC.Exceptions;
using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Expressions;
using TuchinC.AST.Statements;
using System.Runtime.InteropServices;
using TuchinC.Semantic.Types;

namespace TuchinC.Syntax
{
    public partial class Parser
    {
        private Stmt? Declaration()
        {
            try
            {
                if (Match(TokenType.USE)) return ImportDeclaration();
                if (Match(TokenType.STRUCT)) return StructDeclaration();
                if (Match(TokenType.FUN)) return FunctionDeclaration(FunctionType.FUNCTION);
                if (Match(TokenType.LET)) return LetDeclaration();

                return Statement();
            }
            catch (ParseError)
            {
                Synchronize();
                return null;
            }
        }

        public Struct StructDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Требуется имя у структуры");

            Consume(TokenType.LEFT_BRACE, "Требуется '{' поле определения имени структуры");
            List <Function> body = GetStructBody();
            Consume(TokenType.RIGHT_BRACE, "Требуется '}' поле объявления тела структуры");


            return new Struct(name,body);
        }

        private List<Function> GetStructBody()
        {
            List<Function> functions = [];
            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
                functions.Add(FunctionDeclaration(FunctionType.METHOD));


            return functions;
        }

        public Use ImportDeclaration()
        {
            List<Token> path = [];
            while (!Check(TokenType.SEMICOLON)) 
            {
                Token part = Consume(TokenType.IDENTIFIER,"Выражение не определено");
                path.Add(part);
                Match(TokenType.DOT);
                Advance();
            }
            if (path.Count == 0)
                Error(Peek(),"Требуется путь к импорта");

            Consume(TokenType.SEMICOLON,"Требуется ';' после определения импорта");
            

            return new Use(path);
        }

        private Function FunctionDeclaration(FunctionType kind) 
        {
            Token name = Consume(TokenType.IDENTIFIER,$"Требуется название {kind}.");
            List<Token> parameters = GetFunctionParameters(kind);

            Stmt body = GetFunctionBody(kind);

            return new Function(name, parameters, body);
        }

        private List<Token> GetFunctionParameters(FunctionType kind)
        {
            Consume(TokenType.LEFT_PAREN, $"Требуется '(' после имени {kind}");
            List<Token> parameters = [];
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    if(parameters.Count >= 255)
                        Error(Peek(), "Число аргументов функции превышает 255");


                    parameters.Add(
                        Consume(TokenType.IDENTIFIER,"Требуется идентификатор"));
                } while (Match(TokenType.COMMA));
            }
            Consume(TokenType.RIGHT_PAREN, $"Требуется ')' после обьявления параметров");

            return parameters;
        }

        private Stmt GetFunctionBody(FunctionType kind)
        {
            Stmt body;
            if (!Match(TokenType.ARROW))
            {
                Consume(TokenType.LEFT_BRACE, "Требуется '{' после обьявления параметров " + kind);
                List<Stmt?> block = Block();
                body = new Block(block);
            }
            else 
            {
                body = ExpressionStatement();
            }


            return body;
        }

        private Let LetDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Требуется имя переменной");
            ValueType type = GetTypeLet(); 
            Expr? init = Match(TokenType.EQUAL) ? Expression() : null;
            

            Consume(TokenType.SEMICOLON, "Требуется ';' после объявления переменной");
            return new Let(name, type, init);
        }

        private ValueType GetTypeLet()
        {
            if (Match(TokenType.COLON))
            {
                if (Match(TokenType.IDENTIFIER) || Match(TokenType.LITERAL))
                    return new ValueType(Peek().Lexeme,  TypeValue.None);

                Error(Previous(), "Требуется тип после ':'");

            }

            return ValueType.ToEmpty();
        }
    }
}
