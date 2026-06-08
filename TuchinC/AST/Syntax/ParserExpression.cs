using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements;
using TuchinC.AST.Nodes.Expressions.Calles;

namespace TuchinC.Syntax
{
    public partial class Parser
    {
        private Expr Expression() => Assignment();

        private Expr Assignment()
        {
            Expr expr = Tern();

            if (Match(TokenType.EQUAL))
            {
                Token equals = Previous();
                Expr value = Assignment();

                if (expr is Variable variable)
                {
                    Token name = variable.Name;
                    return new Assign(name, value);
                }
                else if (expr is Get get)
                    return new Set(get.Name, get.Object, value);


                Error(equals, "Операция присваивания может быть выполнено только с переменными, полями, или свойствами");
            }

            return expr;
        }

        


        private Expr Tern()
        {
            Expr expr = Binnary();

            if (expr is null)
                throw new ArgumentNullException(nameof(expr), "Выражение не должно быть пустым");

            while (Match(TokenType.QUESTION))
            {
                Expr that = Expression();
                if (Match(TokenType.COLON))
                {
                    Expr _else = Expression();
                    expr = new Ternary(expr, that, _else);
                }
                else
                {
                    Error(Peek(), "Ожидалось ':' после ветви 'то' в тернарном операторе");
                }
            }

            return expr;

        }

       
        private Expr Binnary()
        {
            // Список списков токенов с флагами
            // на является ли текущая бинарная операция логической или обычной
            // и будет ли проверка на стрелучную функцию
            List < (TokenType[], bool, bool) > tokens = [([TokenType.VLINE_VLINE], true, true),
                ([TokenType.AND], true, false), 
                ([TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL], true, false), 
                ([TokenType.VLINE], false, true),
                ([TokenType.XOR], false, false), 
                ([TokenType.AND_BIT], false, false),
                ([TokenType.VLINE], false, true),
                ([TokenType.XOR], false, false),
                ([TokenType.AND_BIT], false, false),
                ([TokenType.GREATER, TokenType.GREATER_EQUAL,
                    TokenType.LESS, TokenType.LESS_EQUAL], true, false),
                ([TokenType.LEFT_OFFSET, TokenType.RIGHT_OFFSET], false, false),
                ([TokenType.MINUS, TokenType.PLUS], false, false ),
                ([TokenType.SLASH, TokenType.STAR], false, false)];

            return ParseBinnary(tokens);
        }

        // Рекурсивно считывание  все бинарные операции по списку кортежа
        private Expr ParseBinnary(in List<(TokenType[], bool, bool)> tokens, int leftIndex = 0)
        {
            int currentIndex = leftIndex;
            Expr expr = leftIndex < tokens.Count-1 ? ParseBinnary(tokens, leftIndex+1) : Unary();
            
            if (Match(tokens[currentIndex].Item1) && (!tokens[currentIndex].Item3 || expr is not ArrowFunction))
            {
                Token @operator = Previous();
                Expr right = Tern();
                expr = tokens[currentIndex].Item2 
                        ? new Logical(expr, @operator, right) 
                        : new Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(TokenType.NOT_BIT, TokenType.BANG, TokenType.PLUS, TokenType.MINUS))
            {
                Token @operator = Previous();
                Expr right = Unary();
                return new Unary(@operator, right);
            }


            return Call();
        }

        


        private Expr Call()
        {
            Expr expr = Primary();

            while (true)
            {
                if (Match(TokenType.LEFT_PAREN))
                    expr = FunctionCall(expr);
                else if (Match(TokenType.LEFT_BRACKET))
                    expr = IteratorCall(expr);
                else if (Match(TokenType.DOT))
                {
                    Token name = Consume(TokenType.IDENTIFIER, "Требуется имя свойства после '.'.");
                    expr = new Get(name, expr);
                }
                else break;
            }
            return expr;
        }



        private FunctionCall FunctionCall(Expr calle)
        {
            List<Expr> args = [];
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    if (args.Count >= 255)
                        Error(Peek(), "Число аргументов функции превышает 255"); 

                    args.Add(Expression());
                }
                while (Match(TokenType.COMMA));
            }

            Token paren = Consume(TokenType.RIGHT_PAREN,
                                   "Требуется ')' после передачи аргументов в функцию");

            return new FunctionCall(calle,paren,args); 
        }
        private IteratorCall IteratorCall(Expr calle)
        {
            Expr? index = null;
            if (!Check(TokenType.RIGHT_BRACKET))
                index = Tern();
           
            if (index is not null)
            {

                Token paren = Consume(TokenType.RIGHT_BRACKET,
                                       "Требуется ']' после передачи индекса в итератор");

                return new IteratorCall(calle, paren, index);
            }
            else
            {
                throw Error(Peek(), "Ожидается выражение но используется как тип");
            }

        }


        private Expr Primary()
        {
            if (Match(TokenType.FALSE)) return new Literal(Previous(), false);
            if (Match(TokenType.TRUE)) return new Literal(Previous(), true);
            if (Match(TokenType.NIL)) return new Literal(Previous(), null);

            if (Match(TokenType.LEFT_BRACKET))
                return new Collection(Previous(), Elements());

            if (Match(TokenType.LITERAL))
                return new Literal(Previous(), Previous().Literal);
            
            if (Match(TokenType.IDENTIFIER))
                return new Variable(Previous());

            if (Match(TokenType.THIS))
                return new This(Previous());

            if (Match(TokenType.VLINE) || Match(TokenType.VLINE_VLINE))
                return Arrow(Previous());

            if (Match(TokenType.LEFT_PAREN))
            {
                var group = new Grouping(Previous(), Expression());
                Consume(TokenType.RIGHT_PAREN, "Требуется ')'");
                return group;
            }


            throw Error(Peek(), "Выражение отсутствует");
        }

        private ArrowFunction Arrow(Token token)
        {
            TokenType type = token.Type;
            List<Token> @params = [];
            if (type == TokenType.VLINE)
               @params = GetParamsArrowFunction();

            if (Match(TokenType.ARROW))
            {
                SyntaxTree input;
                if (Match(TokenType.LEFT_BRACE))
                {
                    List<Stmt?> statements = Block();
                    Block body = new(Previous(), statements);
                    input = body;
                }
                else
                {
                    Expr expr = Expression();
                    input = expr;
                }

                return new ArrowFunction(token, @params, input);
            }

            throw Error(Peek(),"Требуется '=>' при определении стрелочной функции");
        }

        private List<Token> GetParamsArrowFunction() 
        {
            List<Token> parameters = [];
            if (!Check(TokenType.VLINE))
            {
                do
                {
                    if (parameters.Count >= 255)
                        Error(Peek(), "Число аргументов функции превышает 255");

                    parameters.Add(
                        Consume(TokenType.IDENTIFIER, "Требуется идентификатор"));
                }
                while (Match(TokenType.COMMA));
           
            }
            

            Consume(TokenType.VLINE, $"Требуется '|' после обьявления параметров стрелочной функции");
            return parameters;
        }


        private List<Expr> Elements()
        {
            List<Expr> elements = [];
            if (!Check(TokenType.RIGHT_BRACKET))
            {
                do
                {
                    elements.Add(Tern());
                }
                while (Match(TokenType.COMMA));
            }

            Consume(TokenType.RIGHT_BRACKET, 
                "Требуется ']' после передачи элементов в коллекцию");

            return elements;
        } 

    }
}
