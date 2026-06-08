using TuchinC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using TuchinC.AST.Lexical;
using TuchinC.AST.Semantic.Types;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements;

using Function = TuchinC.AST.Nodes.Statements.Function;

namespace TuchinC.Syntax
{
    public partial class Parser
    {

        private readonly List<Modifier> _modifiers = [];

        private Stmt? Declaration()
        {
            try
            {
                if (GetModifier() != null)
                    Modifiers();

                if (Match(TokenType.USE)) return ImportDeclaration(Previous());
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

     

        private void Modifiers()
        {
            while (!IsAtEnd())
            {
                Modifier? modifier = GetModifier();

                if (modifier == null)
                    break;

                AddModifier(modifier);
                Advance();
            }

            if (!Check(TokenType.STRUCT, TokenType.FUN))
                 Error(Peek(), "Модификаторы поддерживают только обьявления структур, функций");
            
        }

        private Modifier? GetModifier() => Peek().Type switch
        {
            TokenType.PUB => new(Peek(),ModifierType.Public),
            TokenType.PRIVATE => new(Peek(), ModifierType.Private),
            TokenType.EXTERN => new(Peek(), ModifierType.Extern),
            _ => null
        }; 


        private void AddModifier(Modifier modifier) => _modifiers.Add(modifier);
        private void ClearModifiers() => _modifiers.Clear();
        

        private Struct StructDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Требуется имя у структуры");

            Consume(TokenType.LEFT_BRACE, "Требуется '{' поле определения имени структуры");
            List <Function> body = GetStructBody();
            Consume(TokenType.RIGHT_BRACE, "Требуется '}' поле объявления тела структуры");

            Struct result = new(name, body, _modifiers);
            ClearModifiers();

            return result;
        }

        private List<Function> GetStructBody()
        {
            List<Function> functions = [];
            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
                functions.Add(FunctionDeclaration(FunctionType.METHOD));

            return functions;
        }

        private Use ImportDeclaration(Token keyword)
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
            

            return new Use(keyword, path);
        }

        private Function FunctionDeclaration(FunctionType kind = FunctionType.FUNCTION) 
        {
            Token name = Consume(TokenType.IDENTIFIER,$"Требуется название {kind}.");
            List<Param> parameters = GetFunctionParameters(kind);
            ValueType @return = GetFunctionReturnType();
            Stmt? body = null;

            if (!Match(TokenType.SEMICOLON))
                body = GetFunctionBody(Previous(), kind);
            
            Function result = new(name, parameters, _modifiers, @return, body);
            ClearModifiers();

            return result;
        }

        private List<Param> GetFunctionParameters(FunctionType kind)
        {
            Consume(TokenType.LEFT_PAREN, $"Требуется '(' после имени {kind}");
            List<Param> parameters = [];
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    if(parameters.Count >= 255)
                        Error(Peek(), "Число аргументов функции превышает 255");
                    
                    Token name = Consume(TokenType.IDENTIFIER, "Требуется идентификатор");
                    ValueType type = GetTypeDeclaration();
                    Param param = new(name, type);
                    
                    parameters.Add(param);

                    GetTypeDeclaration();

                } while (Match(TokenType.COMMA));
            }
            Consume(TokenType.RIGHT_PAREN, $"Требуется ')' после обьявления параметров");

            return parameters;
        }

        private ValueType GetFunctionReturnType()
        {
            if (!Match(TokenType.ARROW_OPERATOR))
                return ValueType.ToEmpty();

            var type = Consume([TokenType.IDENTIFIER, TokenType.LITERAL], "Требуется тип после ':'");
            return new ValueType();
        }

        private Stmt GetFunctionBody(Token keyword, FunctionType kind)
        {
            Stmt body;
            if (!Match(TokenType.ARROW))
            {
                Consume(TokenType.LEFT_BRACE, "Требуется '{' после обьявления параметров " + kind);
                List<Stmt?> block = Block();
                body = new Block(keyword, block);
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
            ValueType type = GetTypeDeclaration(); 
            Expr? init = Match(TokenType.EQUAL) ? Expression() : null;
            

            Consume(TokenType.SEMICOLON, "Требуется ';' после объявления переменной");
            return new Let(name, type.Type, init);
        }

        private ValueType GetTypeDeclaration()
        {
            if (Match(TokenType.COLON))
                return CastTokenToValueType(":");
            

            return ValueType.ToEmpty();
        }

        private ValueType CastTokenToValueType(string word)
        {
            Token type = Peek();
            Consume(TokenType.LITERAL, $"Требуется тип после '{word}'");
            return new ValueType(type.Lexeme);
        }
    }
}
