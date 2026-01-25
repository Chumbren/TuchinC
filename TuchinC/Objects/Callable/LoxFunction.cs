using TuchinC.AST.Expressions;
using TuchinC.AST.Statements;
using TuchinC.Exceptions;
using TuchinC.Interpreters;
using TuchinC.Lexical;
using TuchinC.Objects.Callable.ClassInstance;
using TuchinC.Semantic;
using Environment = TuchinC.Semantic.Environment;

namespace TuchinC.Objects.Callable
{
    public class LoxFunction(Function declaration, Environment closure, FunctionType type) : ILoxCallable
    {
        public readonly FunctionType Type = type; 

        private readonly Function _declaration = declaration;
        private readonly Environment _closure = closure;
        public int Arity() => _declaration.Params.Count;

        public object? Call(Interpreter interpreter, List<object?> args)
        {
            Environment environment = new(_closure);
            List<Token> parameters = _declaration.Params;


            for (int i = 0; i < parameters.Count; i++)
                environment.Define(parameters[i].Lexeme, args[i]);


            try
            {
                List<Stmt?> statements = GetFunctionBody();
                interpreter.ExecuteBlock(statements, environment);
            }
            catch (Returned _return)
            {
                if (Type == FunctionType.INITIALIZER) return _closure.GetAt(0, "this");

                return _return.Value;

            }

            if (Type == FunctionType.INITIALIZER) return _closure.GetAt(0, "this");

            return null;
        }

        private List<Stmt?> GetFunctionBody()
        {
            List<Stmt?> statements;

            if (_declaration.Body is Block block)
                statements = block.Statements;
            else
                statements = GetExprBody();

            return statements;
        }


        private List<Stmt?> GetExprBody() 
        {

            var body = _declaration.Body;
            Expr returned;

            if (body is Expression expression)
                returned = expression.Value;
            else if (body is Expr expr)
                returned = expr;
            else
                throw new RuntimeError(new Token(TokenType.FUN), "Неизвестный тип input");

            Return @return = new(new Token(TokenType.RETURN), returned);


            return [@return];
        }

        public LoxFunction Bind(LoxInstance instance)
        {
            Environment env = new(_closure);
            env.Define("this", instance);
            return new LoxFunction(_declaration,env, Type);
        }


        public override string ToString() 
        {
            string name = _declaration.Name.Lexeme;
            if (name.Contains("ArrowFunction", StringComparison.CurrentCulture)) 
                return "<fn ArrowFunction >";

            return $"<fn {name} >";
        }
    }
}
