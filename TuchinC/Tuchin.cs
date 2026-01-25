
using System.Text;
using TuchinC.Lexical;
using TuchinC.Interpreters;
using TuchinC.Exceptions;
using TuchinC.AST.Statements;
using TuchinC.Semantic;
using TuchinC.Syntax;
using TuchinC.Semantic.Types.Exceptions;

namespace TuchinC
{
    public static class Tuchin
    {

        public static readonly string Path = String.Empty;

        private static readonly Interpreter _interpreter = new();
        private static bool _hadError = false;
        private static bool _hadRuntimeError = false;


        public static void RunPromt()
        {
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null) break;
                Run(line);

                _hadError = false;
            }
        }

        public static void RunFile(string path)
        {
            byte[] file = File.ReadAllBytes(path);
            string source = Encoding.UTF8.GetString(file);

            Run(source);
            if (_hadError) System.Environment.Exit(65);
            if (_hadRuntimeError) System.Environment.Exit(70);
        }


        private static void Run(string source)
        {
            List<Token> tokens = Scan(source);
            List<Stmt?> statements = Parse(tokens);

            if (_hadError) return;

            Resolve(statements);

            if (_hadError) return;

            _interpreter.Interpret(statements);
        }

        private static List<Token> Scan(string source)
        {
            Scanner scanner = new(source);
            List<Token> tokens = scanner.ScanTokens();

            return tokens;
        }

        private static List<Stmt?> Parse(List<Token> tokens)
        {
            Parser parser = new(tokens);
            List<Stmt?> statements = parser.Parse();

            return statements;
        }

        private static void Resolve(List<Stmt?> stmts)
        {
            Resolver resolver = new(_interpreter);
            resolver.Analize(stmts);
        }

        public static void RuntimeError(RuntimeError error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\r\nRuntime -> [ line {error.Token.Line} ]{error.Message}\n\r\n\r\n");
            Console.ForegroundColor = ConsoleColor.White;
            _hadRuntimeError = true;
        }
        public static void Error(int line, string message) => Report(line, "", message);

        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.EOF)
                Report(token.Line, " at end", message);
            else
                Report(token.Line, $" at '{token.Lexeme}'", message);
        }
        public static void Error(Token token, TypeException ex) 
            => Error(token,ex.Message);

        private static void Report(int line, string where, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{line}] Error {where}: {message}");
            _hadError = true;
        }

    }
}
