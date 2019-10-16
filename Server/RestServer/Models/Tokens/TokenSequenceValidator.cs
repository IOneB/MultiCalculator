using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestServer.Models.Tokens
{
    /// <summary>
    /// Класс, проверяющий последовательность токенов в выражении формулы
    /// </summary>
    public class TokenSequenceValidator : IValidator<(string, string)>
    {
        /// <summary>
        /// Символизирует первый элемент
        /// </summary>
        public const string first = "first";
        /// <summary>
        /// Символизирует последний элемент
        /// </summary>
        public const string last = "last";

        /// <summary>
        /// Правила сопоставления элементов в последовательности. Для каждого элемента допускается набор значений
        /// </summary>
        private readonly IReadOnlyDictionary<string, IReadOnlyList<string>> tokenSequenceRules;

        public TokenSequenceValidator()
        {
             tokenSequenceRules = new Dictionary<string, IReadOnlyList<string>>
             {
                 [first] = new List<string> { "Open" + nameof(BracketToken), nameof(FormulaToken), nameof(NumericToken) },
                 ["Open" + nameof(BracketToken)] = new List<string> { "Open" + nameof(BracketToken), "Close" + nameof(BracketToken), nameof(FormulaToken), nameof(NumericToken) },
                 ["Close" + nameof(BracketToken)] = new List<string> { "Close" + nameof(BracketToken), nameof(OperatorToken)},
                 [nameof(FormulaToken)] = new List<string> { "Close" + nameof(BracketToken), nameof(OperatorToken) },
                 [nameof(NumericToken)] = new List<string> { "Close" + nameof(BracketToken), nameof(OperatorToken) },
                 [nameof(OperatorToken)] = new List<string> { "Open" + nameof(BracketToken), nameof(FormulaToken), nameof(NumericToken) },
                 [last] = new List<string> { "Close" + nameof(BracketToken), nameof(FormulaToken), nameof(NumericToken) },
             };
        }

        public bool Validate((string, string) entity)
        {
            if (!tokenSequenceRules.ContainsKey(entity.Item1))
                return false;

            return tokenSequenceRules[entity.Item1].Contains(entity.Item2);
        }
    }
}
