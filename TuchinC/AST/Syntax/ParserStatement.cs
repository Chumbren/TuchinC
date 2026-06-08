using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements;

namespace TuchinC.Syntax
{
    public partial class Parser
    {
        private Stmt Statement()
        {
            if (Match(TokenType.FOR)) return ForStatement();
            if (Match(TokenType.IF)) return IfStatement();
            if (Match(TokenType.SWITCH)) return SwitchStatement(Previous());
            if (Match(TokenType.RETURN)) return ReturnStatement();
            if (Match(TokenType.LOOP)) return LoopStatement();
            if (Match(TokenType.PRINT)) return PrintStatement();
            if (Match(TokenType.LEFT_BRACE)) return new Block(Previous(),Block());

            StatementError();

            return ExpressionStatement();
        }

        private void StatementError()
        {
            if (Match(TokenType.ELIF)) throw Error(Previous(), "Ветка elif не может идти в отрыве от if");
            if (Match(TokenType.ELSE)) throw Error(Previous(), "Ветка else не может идти в отрыве от if");
            if (Match(TokenType.CASE)) throw Error(Previous(), "Оператор case не может идти в отрыве от оператора switch");
            if (Match(TokenType.DEFAULT)) throw Error(Previous(), "Оператор default не может идти в отрыве от оператора switch");

        }


        private Return ReturnStatement()
        {
            Token keyword = Previous();
            Expr? value = null;
            if (!Check(TokenType.SEMICOLON))
                value = Expression();

            Consume(TokenType.SEMICOLON, "Требуется ';' после возращаемого значения");
            return new Return(keyword, value);
        }

        private Stmt ForStatement()
        {
            Token keyword = Previous();
            Stmt? init = GetInitLoopFor();
            Expr? condition = GetConditionLoopFor();
            Expr? increament = GetIncreamentLoopFor();

            return DesugarLoopFor(keyword, init, condition, increament);
        }

        private Stmt? GetInitLoopFor()
        {
            Stmt? init;

            if (Match(TokenType.SEMICOLON))
                init = null;
            else if (Match(TokenType.LET))
                init = LetDeclaration();
            else
                init = ExpressionStatement();

            return init;
        }

        private Expr? GetConditionLoopFor()
        {

            Expr? condition = !Check(TokenType.SEMICOLON) ? Expression() : null;
            Consume(TokenType.SEMICOLON, "Требуется ';' после условия цикла");

            return condition;
        }

        private Expr? GetIncreamentLoopFor()
        {
            Expr? increament = !Check(TokenType.RIGHT_PAREN) ? Expression() : null;
            Consume(TokenType.RIGHT_PAREN, "Требуется ')' после фазы предложений цикла for ");

            return increament;
        }

        private Stmt DesugarLoopFor(Token keyword, Stmt? init, Expr? condition, Expr? increament)
        {
            Stmt @for = Statement();

            if (increament != null)
                @for = new Block(keyword, [@for,new Expression(increament)]);

            condition ??= new Literal(new Token(TokenType.LITERAL), true);
            @for = new Loop(keyword, condition, @for);

            if (init != null)
                @for = new Block(keyword, [init, @for]);


            return @for;

        }

        private Switch SwitchStatement(Token keyword)
        {
            Expr condition = Expression();
            Consume(TokenType.LEFT_PAREN, "Требуется '{' после 'switch'");

            List<Case> cases = GetSwitchCases(); 
            Block? @default = GetDefaultCase();
     
            Consume(TokenType.RIGHT_PAREN, "Требуется '}' для завешения блока 'switch'");


            return new Switch(keyword, condition, cases, @default);
        }

        

        private List<Case> GetSwitchCases() 
        {
            List<Case> cases = [];

            while (Match(TokenType.CASE))
                cases.Add(GetCase());
             
            
            return cases;
        }

        private Case GetCase()
        {
            Expr expr = Primary();

            if (expr is not Literal)
                throw Error(Peek(), "Выражение в case дожно быть константным");

            Literal value = (Literal)expr;

            Consume(TokenType.LEFT_PAREN, "Требуется ':' после 'case'");
            List<Stmt?> body = GetCaseBody();
            Block block = new(Previous(), body);
            Case @case = new(value, block);


            return @case;
        }


        private Block? GetDefaultCase()
        {
            Block? @default = null;
            if (Match(TokenType.DEFAULT))
            {
                List<Stmt?> body = GetCaseBody();
                @default = new Block(Previous(), body);
            }

            return @default;
        }

        private List<Stmt?> GetCaseBody() => GetBlockBody(TokenType.BREAK);
           


        private If IfStatement()
        {
            Token keyword = Previous();
            Expr condition = Expression();
            if (condition is not Logical)
                Tuchin.Error(condition.Keyword, "Условие не является логическим");

            Stmt thenBranch = Statement();
            List<Elif> elifBranches = GetElifBranches();
 
            Stmt? elseBranch = null;

            if (Match(TokenType.ELSE))
                elseBranch = Statement();


            return new If(keyword, condition, thenBranch, elifBranches, elseBranch);
        }

        private List<Elif> GetElifBranches()
        {
            List<Elif> elifs = [];
            while (Match(TokenType.ELIF)) 
            {
                Expr condition = Expression();
                Stmt branch = Statement();
                Elif elif = new(condition,branch);
                elifs.Add(elif);
            }


            return elifs;
        }




        private Loop LoopStatement()
        {
            Expr? condition = null;

            if (Peek() is not { Type:TokenType.LEFT_BRACE })
            {
               condition = Expression();
            }
            Stmt? body = Statement();


            return new Loop(Previous(), condition, body);
        }


        private Expression ExpressionStatement()
        {
            Expr expr = Expression();
            Consume(TokenType.SEMICOLON, "Требуется ';' после выражения.");
            return new Expression(expr);
        }

        private List<Stmt?> Block()
        {
            List<Stmt?> statements = GetBlockBody(TokenType.RIGHT_BRACE);

            Consume(TokenType.RIGHT_BRACE, "Требуется '}' после блока");
            return statements;
        }

        private List<Stmt?> GetBlockBody(TokenType end)
        {
            List<Stmt?> statements = [];

            while (!Check(end) && !IsAtEnd())
                statements.Add(Declaration());

            return statements;
        }

        private Print PrintStatement()
        {
            Token keyword = Previous();
            Expression expr = ExpressionStatement();

            return new Print(keyword, expr.Value);
        }

    }
}
