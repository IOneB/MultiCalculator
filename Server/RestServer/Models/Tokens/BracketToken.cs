using System.Collections.Generic;

namespace RestServer.Models.Tokens
{
    /// <summary>
    /// Токен, представляющий скобки
    /// </summary>
    public class BracketToken : Token
    {
        public bool IsOpening { get; }
        public bool IsClosing => !IsOpening;
        public override string Name => (IsOpening ? "Open": "Close") + base.Name;

        internal BracketToken(string token) : base(token)
        {
            if (token == "(")
                IsOpening = true;
        }

        public override void CreateRPN(Stack<Token> stack, List<Token> tokens)
        {
            if (IsOpening) // Запоминаем все, что внутри скобки
            {
                stack.Push(this);
            }
            else // Вся внутрення часть скобок отправляется в результирующий список
            {
                while (!(stack.Peek() is BracketToken bracket && bracket.IsOpening))
                {
                    tokens.Add(stack.Pop());
                }
                stack.Pop(); // От самой скобки тоже нужно избавиться, она больше не нужна
            }
        }
    }
}