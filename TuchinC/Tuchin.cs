
using System.Text;
using TuchinC.Exceptions;
using TuchinC.Semantic;
using TuchinC.Syntax;
using TuchinC.GenerateByte;
using TuchinC.AST.Lexical;
using TuchinC.AST.Semantic.Types.Exceptions;
using TuchinC.AST.Nodes.Statements;
using TuchinC.GenerateByte.Disassemble;

namespace TuchinC
{
    public static class Tuchin
    {
        private static bool _hadError = false;
        private static bool _hadRuntimeError = false;

        public static void RunFile(string path)
        {
            string source = File.ReadAllText(path);

            Run(path, source);
            if (_hadError) System.Environment.Exit(65);
            if (_hadRuntimeError) System.Environment.Exit(70);
        }


        public static void Run(string project, string source)
        {
            List<Token> tokens = Scan(source);
            List<Stmt?> statements = Parse(tokens);

            if (_hadError) return;

            Resolve(statements);

            if (_hadError) return;

            var bytecode = Generate(project, statements);
        }
        public static string? RunWithDisassembler(string project, string source)
        {
            List<Token> tokens = Scan(source);
            List<Stmt?> statements = Parse(tokens);

            if (_hadError) return null;

            Resolve(statements);

            if (_hadError) return null;

            var bytecode = Generate(project, statements);
            Console.WriteLine("BYTECODE");
            Console.WriteLine(new String('=', 100));
            foreach (var item in bytecode)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(new String('=', 100));

            Disassembler disassembler = new([.. bytecode]);
            var result = disassembler.Disassemble();

            return result;
        }

        public static List<Token> Scan(string source)
        {
            Scanner scanner = new(source);
            List<Token> tokens = scanner.ScanTokens();

            return tokens;
        }

        public static List<Stmt?> Parse(List<Token> tokens)
        {
            Parser parser = new(tokens);
            List<Stmt?> statements = parser.Parse();

            return statements;
        }

        public static void Resolve(List<Stmt?> stmts)
        {
            Resolver resolver = new();
            resolver.Analize(stmts);
        }

        public static IReadOnlyList<byte> Generate(string project, List<Stmt?> stmts)
        {
            Generator generator = new(project, stmts);
            return generator.Generate();
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
