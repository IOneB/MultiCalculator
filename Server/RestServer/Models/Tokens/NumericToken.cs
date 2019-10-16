using System.Collections.Generic;

namespace RestServer.Models.Tokens
{
    /// <summary>
    /// Токен, представляющий вещественное число в формуле
    /// </summary>
    public class NumericToken : Token, ITokenCalculator
    {
        /// <summary>
        /// Числовое представление токена
        /// </summary>
        public double Result { get; }

        internal NumericToken(string token, double result) : base(token)
        {
            Result = result;
        }

        public override void CreateRPN(Stack<Token> stack, List<Token> tokens)
        {
            tokens.Add(this);  // Числа и символы просто добавляются в контейнер
        }

        public void Calculate(Stack<double> stack)
        {
            stack.Push(Result); // Число необходимо просто запомнить
        }

    }
}