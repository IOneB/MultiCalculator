using System;
using System.Collections.Generic;

namespace RestServer.Models.Tokens
{
    /// <summary>
    /// Токен, представляющий бинарную операцию над числовыми токенами
    /// </summary>
    public class OperatorToken : Token, ITokenCalculator
    {
        internal OperatorToken(string token) : base(token)
        {
        }

        public void Calculate(Stack<double> stack)
        {
            double rightOperand; //в некоторых операциях важен порядок расчета, так что правый операнд сохраняем
            switch (Value)
            {
                case "+":
                    stack.Push(stack.Pop() + stack.Pop());
                    break;
                case "*":
                    stack.Push(stack.Pop() * stack.Pop());
                    break;
                case "-":
                    rightOperand = stack.Pop();
                    stack.Push(stack.Pop() - rightOperand);
                    break;
                case "/":
                    rightOperand = stack.Pop();
                    if (rightOperand != 0.0)
                        stack.Push(stack.Pop() / rightOperand);
                    else
                        throw new DivideByZeroException("Ошибка. Деление на ноль");
                    break;
                default:
                    throw new ArgumentException("Неизвестный оператор");
            }
        }

        public override void CreateRPN(Stack<Token> stack, List<Token> tokens)
        {
            while (stack.Count > 0 &&
                Operator.Priority(stack.Peek().Value) >= Operator.Priority(Value))
            {
                tokens.Add(stack.Pop()); // Отправляем в результирующий список все операторы, приоритет которых не превышает текущий
            }

            stack.Push(this); // Запоминаем токен
        }
    }
}