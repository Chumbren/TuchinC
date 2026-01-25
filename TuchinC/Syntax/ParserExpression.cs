using TuchinC.AST;
using TuchinC.AST.Expressions;
using TuchinC.AST.Expressions.Calles;
using TuchinC.AST.Statements;
using TuchinC.Lexical;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    return new Set(get.Object, get.Name, value);


                Error(equals, "Операция присваивания может быть выполнено только с переменными, полями, или свойствами");
            }

            return expr;
        }

        


        private Expr Tern()
        {
            Expr expr = Or();

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

        private Expr Or()
        {
            Expr expr = And();

            while (Match(TokenType.VLINE_VLINE) && expr is not ArrowFunction)
            {
                Token _operator = Previous();
                Expr right = And();
                expr = new Logical(expr, _operator, right);
            }

            return expr;
        }


        private Expr And()
        {
            Expr expr = Equality();

            while (Match(TokenType.AND))
            {
                Token _operator = Previous();
                Expr right = Equality();
                expr = new Logical(expr, _operator, right);
            }

            return expr;
        }


        private Expr Equality()
        {
            Expr expr = BitTerm();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token _operator = Previous();
                Expr right = BitTerm();
                expr = new Binary(expr, _operator, right);
            }

            return expr;
        }

        private Expr BitTerm()
        {
            Expr expr = BitXor();

            while (Match(TokenType.VLINE) && expr is not ArrowFunction)
            {
                Token _operator = Previous();
                Expr right = BitXor();
                expr = new Binary(expr, _operator, right);
            }


            return expr;

        }

        private Expr BitXor()
        {
            Expr expr = BitFactor();

            while (Match(TokenType.XOR))
            {
                Token _operator = Previous();
                Expr right = BitFactor();
                expr = new Binary(expr, _operator, right);
            }


            return expr;
        }

        private Expr BitFactor()
        {
            Expr expr = Comprasion();

            while (Match(TokenType.AND_BIT))
            {
                Token _operator = Previous();
                Expr right = Comprasion();
                expr = new Binary(expr, _operator, right);
            }


            return expr;
        }

        private Expr Comprasion()
        {
            Expr expr = BitOffset();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL,
                TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token _operator = Previous();
                Expr right = BitOffset();
                expr = new Binary(expr, _operator, right);
            }

            return expr;
        }


        private Expr BitOffset()
        {
            Expr expr = Term();

            while (Match(TokenType.LEFT_OFFSET, TokenType.RIGHT_OFFSET))
            {
                Token _operator = Previous();
                Expr right = Term();
                expr = new Binary(expr, _operator, right);
            }


            return expr;

        }

        private Expr Term()
        {
            Expr expr = Factor();

            while (Match(TokenType.MINUS, TokenType.PLUS))
            {

                Token _operator = Previous();
                Expr right = Factor();
                expr = new Binary(expr, _operator, right);
            }


            return expr;
        }

        private Expr Factor()
        {
            Expr expr = Unary();


            while (Match(TokenType.SLASH, TokenType.STAR))
            {

                Token _operator = Previous();
                Expr right = Unary();
                expr = new Binary(expr, _operator, right);
            }


            return expr;
        }

        private Expr Unary()
        {
            if (Match(TokenType.NOT_BIT, TokenType.BANG, TokenType.PLUS, TokenType.MINUS))
            {
                Token _operator = Previous();
                Expr right = Unary();
                return new Unary(_operator, right);
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
                    expr = new Get(expr,name);
                }
                else break;
            }
            return expr;
        }



        private FunctionCall FunctionCall(Expr calle)
        {
            var a = calle;
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
            if (Match(TokenType.FALSE)) return new Literal(false);
            if (Match(TokenType.TRUE)) return new Literal(true);
            if (Match(TokenType.NIL)) return new Literal(null);

            if (Match(TokenType.LEFT_BRACKET))
                return new Collection(Elements());

            if (Match(TokenType.LITERAL))
                return new Literal(Previous().Literal);

            if (Match(TokenType.IDENTIFIER))
                return new Variable(Previous());

            if (Match(TokenType.THIS))
                return new This(Previous());

            if (Match(TokenType.VLINE) || Match(TokenType.VLINE_VLINE))
                return Arrow(Previous().Type);

            if (Match(TokenType.LEFT_PAREN))
            {
                var group = new Grouping(Expression());
                Consume(TokenType.RIGHT_PAREN, "Требуется ')'");
                return group;
            }


            throw Error(Peek(), "Выражение отсутствует");
        }

        private ArrowFunction Arrow(TokenType type)
        {
            
            List<Token> _params = [];
            if (type == TokenType.VLINE)
               _params = GetParamsArrowFunction();

            if (Match(TokenType.ARROW))
            {
                ISyntaxTree input;
                if (Match(TokenType.LEFT_BRACE))
                {
                    List<Stmt?> statements = Block();
                    Block body = new(statements);
                    input = body;
                }
                else
                {
                    Expr expr = Expression();
                    input = expr;
                }

                

                return new ArrowFunction(_params, input);
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
