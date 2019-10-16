using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestServer.Models.Tokens
{
    /// <summary>
    /// Базовое представление токена в формуле (переменная, оператор, скобка...)
    /// </summary>
    public abstract class Token
    {
        /// <summary>
        /// Строковое представление токена
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Наименование токена
        /// </summary>
        public virtual string Name => GetType().Name;
        protected Token(string token)
        {
            Value = token;
        }

        /// <summary>
        /// Метод токена, участвующий в генерации Обратной Польской Записи по алгоритму Дейкстры
        /// </summary>
        /// <param name="stack">Стек, содержащий некоторые сохраненные значения</param>
        /// <param name="tokens">Результирующая последовательность токенов</param>
        public abstract void CreateRPN(Stack<Token> stack, List<Token> tokens);

        /// <summary>
        /// Статический метод, создающий токен на основе полученной строки.
        /// </summary>
        /// <param name="token">Строковое представление токена</param>
        /// <returns></returns>
        public static Token CreateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("token must have value", nameof(token));
            }
            token = token.Trim();

            if (token == "(" || token == ")")
                return new BracketToken(token);

            else if (Operator.GetAll.Select(o => o.Name).Contains(token))
                return new OperatorToken(token);

            else if (double.TryParse(token, out double result))
                return new NumericToken(token, result);

            else if (token.ToList().All(c => char.IsLetterOrDigit(c)) && char.IsLetter(token[0]))
                return new FormulaToken(token);

            else return new UnknownToken();
        }

        public override string ToString() => Value;

        public override bool Equals(object obj)
        {
            return obj is Token token &&
                   Value == token.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + EqualityComparer<string>.Default.GetHashCode(Value);
        }
    }
}
