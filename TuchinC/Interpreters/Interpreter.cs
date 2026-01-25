using TuchinC.Exceptions;
using TuchinC.Lexical;
using TuchinC.AST.Expressions;
using TuchinC.AST.Statements;
using Environment = TuchinC.Semantic.Environment;
using TuchinC.Objects.Globals.Functions;

namespace TuchinC.Interpreters
{
    public partial class Interpreter
    {
        private Environment _environment = new();
        private readonly Dictionary<Expr,int?> _locals = [];
        internal readonly Environment Globals = new();


        public Interpreter() 
        {
            RegisterGlobals();
            _environment = Globals;
        }
        private void RegisterGlobals()
        {
            
            Globals.Define("clock", new ClockGlobal());
            Globals.Define("print", new PrintGlobal());
            Globals.Define("input", new InputGlobal());

        }

        public void Interpret(List<Stmt?> statements) 
        {
            try
            {
                Console.WriteLine("Interpreter:\r\n\r\n");
                
                foreach (Stmt? statement in statements) 
                    Execute(statement);
            }
            catch (RuntimeError error)
            {
                Tuchin.RuntimeError(error);
            }
        }


        internal void Resolve(Expr expr, int depth) => _locals.Add(expr,depth);

        private static string Stringify(object? obj)
        {
            if (obj == null) return "nil";

            if(obj is double number)
            {
                string text = number.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text[..^2];
                }
                return text;
            }
            return obj.ToString() ?? "nil";
        }


        private static void CheckNumberOperand(Token _operator,object operand)
        {
            if (operand is double) return;
            throw new RuntimeError(_operator,"Операнд не является числом.");
        }

        private static void CheckNumberOperands(Token _operator, 
            object? left,object? right)
        {
            if (left is double && right is double) return;
            throw new RuntimeError(_operator, "Operands must be a numbers.");
        }

        private static bool IsEqual(object? a, object? b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }


        


        private static bool IsTruthy(object? obj) 
        {
            if (obj == null) return false;
            if (obj is bool boolean) return boolean;
            return true;
        }

        
    }
}
