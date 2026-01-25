using TuchinC.AST.Statements;
using TuchinC.Exceptions;
using TuchinC.Lexical;

namespace TuchinC.Syntax
{
    public partial class Parser(List<Token> tokens)
    {
        private readonly List<Token> _tokens = tokens;
        private int _current = 0;

        

        public List<Stmt?> Parse()
        {
            List<Stmt?> statements = [];
            while (!IsAtEnd()) 
                statements.Add(Declaration());

            return statements;
        }

        

        

        private Token Consume(TokenType type,string message)
        {
            if (Check(type)) return Advance();

            throw Error(Peek(),message);

        }

        /// <summary>
        ///     Метод синхронизации состояния при ошибки синтаксиса. При вызове игнорирует все операторы до ';'
        /// </summary>
        private void Synchronize()
        {
            if (Previous().Type == TokenType.SEMICOLON) return;
            
            switch (Peek().Type)
            {
                case TokenType.STRUCT:
                case TokenType.FUN:
                case TokenType.FOR:
                case TokenType.IF:
                case TokenType.RETURN:
                case TokenType.LET:
                case TokenType.LOOP:
                    return;
            }


            Advance();
        }

        private static ParseError Error(Token token, string message)
        {
            Tuchin.Error(token,message);
            return new ParseError(message);
        }

        /// <summary>
        ///     Проверка на соответствие одного из типов из массива types с текущим типом токена 
        ///     со смещением счетчика на 1 при положительной проверке
        /// </summary>
        /// <returns>
        ///     True если один из типов является равным типу текущего токена; иначе false
        /// </returns>
        private bool Match(params TokenType[] types)
        {
            foreach(TokenType type in types) 
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        ///     Проверка на соответствие типа текущего токена с type
        /// </summary>
        /// <returns>
        ///     True если тип текущего токена совпадает с type и текущий токен не EOF
        /// </returns>
        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }



        /// <summary>
        ///     Извлекает текущий токен со смещением счетчика на 1 если его тип не равен EOF
        /// </summary>
        /// <returns>
        ///     Текущий Token
        /// </returns>
        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        /// <summary>
        ///     Проверка на конец анализа
        /// </summary>
        /// <returns>
        ///     True если текущий токен является EOF; иначе false  
        /// </returns>
        private bool IsAtEnd() => Peek().Type == TokenType.EOF;


        /// <summary>
        ///     Извлекает текущий токен
        /// </summary>
        /// <returns> Текущий Token </returns>
        private Token Peek() => _tokens[_current];


        /// <summary>
        ///    Извлечение прошлого токена
        /// </summary>
        /// <returns> Прошлый Token </returns>
        private Token Previous() => _tokens[_current - 1];

    }
}
