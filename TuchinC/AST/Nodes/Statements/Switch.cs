using TuchinC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuchinC.AST.Lexical;
using TuchinC.AST.Nodes.Expressions;
using TuchinC.AST.Nodes.Statements.Visitors;

namespace TuchinC.AST.Nodes.Statements
{
    public class Switch : Stmt
    {
        //private bool _isNullableCheck = false;
        private Block? _nullable = null;
        private readonly Dictionary<int, Block> _hashNumber = [];
        private readonly Dictionary<string, Block> _hashString = [];
        private readonly Block? _default = null;
        private Block[] _jump = [];

        public readonly Expr Condition;
        public readonly List<Case> Cases = [];

        public Switch(Token keyword, Expr condition,List<Case> cases, Block? @default):base(keyword)
        {
            Cases = cases;
            Condition = condition;
            _default = @default;

            if(cases.Count < 5) SetStrategyesCases();
            
        }

        private void SetStrategyesCases()
        {
            List<Block> jump = [];
            foreach (var @case in Cases)
            {
                object? value = @case.Value.Value;

                if (value is int number)
                {
                    if (jump.Count == 0 || jump.Count == number - 1)
                        jump.Add(@case.Body);
                    else
                        _hashNumber.Add(number, @case.Body);
                }
                else if (value is string str)
                    _hashString.Add(str, @case.Body);
                else if (value is null)
                {
                    //_isNullableCheck = true;
                    _nullable = @case.Body;
                }
            }


            _jump = [.. jump];
        }

        //public Stmt? RunStrategy(object? value)
        //{
        //    if (Cases.Count == 0)
        //        return null;

        //    if(Cases.Count < 5) 
        //    {
        //        If @if = GetSmallCases(value);
        //        return @if;
        //    }
            
        //    if (value is int number)
        //    {
        //        if (number > _jump.Length + 1)
        //            return _jump[number];
        //        else
        //            return _hashNumber[number];
        //    }
        //    else if (value is string key)
        //        return _hashString[key];
        //    else if (value == null && _isNullableCheck)
        //        return _nullable;
        //    else if (_default != null)
        //        return _default;
        //    else
        //        return null;

        //}

        //private If GetSmallCases(object? value)
        //{
        //    Logical conditionIf = ConvertCaseToConditionIf(value, Cases[0].Value.Value);
        //    Block block = Cases[0].Body;

        //    List<Elif> elifs = GetSmallCaseElifs(value);
            

        //    return new If(new Token(TokenType.IF), conditionIf,block, elifs, _default);
        //}

        //private List<Elif> GetSmallCaseElifs(object? value)
        //{
        //    List<Elif> elifs = [];

        //    for (int i = 1; i < Cases.Count; i++)
        //    {
        //        Logical conditionElif = ConvertCaseToConditionIf(value, Cases[i].Value.Value);
        //        Elif elif = new(conditionElif, Cases[i].Body);
        //        elifs.Add(elif);
        //    }

        //    return elifs;
        //}

        //private static Logical ConvertCaseToConditionIf(object? value, object? @case)=> new Logical(
        //        new Literal(value),
        //        new Token(TokenType.EQUAL_EQUAL),
        //        new Literal(@case));
        

        public override void Accept(IVisitor visitor) => visitor.VisitSwitchStmt(this);
    }
}
