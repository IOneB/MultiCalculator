using CommonLibrary.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RestServer.Models.Tokens
{
    /// <summary>
    /// Агрегатор токенов, содержит методы генерации ОПЗ и расчета значений
    /// </summary>
    public class TokenCollection : IEnumerable<Token>
    {
        private List<Token> _innerList; // Внутренний список токенов
        private bool isRPNConfigured = false; // Флаг конфигурации токенов согласно ОПЗ

        public int Count => _innerList?.Count ?? 0;
        public Token this[int i] => _innerList[i];


        /// <summary>
        /// Список всех токенов, представляющих переменные
        /// </summary>
        public IEnumerable<Token> GetAllFormula => _innerList.Where(t => t is FormulaToken);

        public TokenCollection()
        {
            _innerList = new List<Token>();
        }

        /// <summary>
        /// Метод, приводящий инфиксную запись токенов к ОПЗ
        /// </summary>
        public void CreateRPN()
        {
            Stack<Token> stack = new Stack<Token>(); // Промежуточный стек
            var newList = new List<Token>(); // Список токенов в форме ОПЗ
            _innerList.ForEach(token => token.CreateRPN(stack, newList)); // Каждый токен влияет на стек и список по своему
            while (stack.Count > 0)
                newList.Add(stack.Pop());
            _innerList = newList;
            isRPNConfigured = true; // Конфигурация прошла успешно
        }

        /// <summary>
        /// Метод расчета итогового значения формулы. Перед вызовом выражение должно быть приведено к форме ОПЗ
        /// </summary>
        /// <param name="formulas">Список формул для подстановки вместо переменных</param>
        /// <returns></returns>
        public double Calculate(List<ServerFormula> formulas)
        {
            if (!isRPNConfigured)
                throw new InvalidOperationException("Перед расчетом значения выражения, необходимо привести его к виду ОПЗ");

            Stack<double> stack = new Stack<double>();
            ReplaceFormula(formulas?.ToDictionary(f => f.Name, f => f.Value));  // Заменяем переменные на числовые токены

            List<ITokenCalculator> tokenCalculators = _innerList  // Получаем список токенов, участвующих в расчете
                .Select(token => token as ITokenCalculator)
                .Where(token => token != null)
                .ToList();

            foreach (var token in tokenCalculators)
                token.Calculate(stack); 

            return stack.Pop(); // Результат
        }

        /// <summary>
        /// Добавляет элементы указанной коллекции строк в конец списка в качестве токенов
        /// </summary>
        /// <param name="stringTokens"></param>
        public void AddMany(IEnumerable<string> stringTokens)
        {
            foreach (var item in stringTokens)
            {
                _innerList.Add(Token.CreateToken(item));
            }
        }

        /// <summary>
        /// Заменяет все переменные на эквивалентные числовые значения
        /// </summary>
        /// <param name="formulas"></param>
        private void ReplaceFormula(Dictionary<string, double> formulas)
        {
            var exception = new ArgumentException("Given formulas doesn't cover all the unknown values in expression", nameof(formulas)); ;

            if (formulas is null || formulas.Count == 0)
            {
                if (GetAllFormula.Count() != 0)
                    throw exception;
                else return;
            }

            for (int i = 0; i < _innerList.Count; i++)
            {
                var formula = _innerList[i].Value;
                if (formulas.ContainsKey(formula))
                    _innerList[i] = new NumericToken(_innerList[i].Value, formulas[formula]);
            }

            if (GetAllFormula.Count() > 0)
                throw exception;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return ((IEnumerable<Token>)_innerList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Token>)_innerList).GetEnumerator();
        }
    }
}
