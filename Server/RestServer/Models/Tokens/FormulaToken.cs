using System.Collections.Generic;

namespace RestServer.Models.Tokens
{
    /// <summary>
    /// Токен, представляющий какую-либо переменную. Последовательность символов и цифр
    /// </summary>
    public class FormulaToken : Token
    {
        internal FormulaToken(string token) : base(token) { }

        public override void CreateRPN(Stack<Token> stack, List<Token> tokens)
        {
            tokens.Add(this); // Числа и символы просто добавляются в стек
        }
    }
}