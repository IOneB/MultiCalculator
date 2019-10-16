using RestServer.Models;
using RestServer.Models.Tokens;
using RestServer.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestServer.Services.Implementations
{
    /// <summary>
    /// Класс, реализующий валидацию формулы на стороне сервера
    /// </summary>
    public class StandartValidator : IFormulaValidator
    {
        public ITokenDiscriminator TokenDiscriminator { get; }

        public StandartValidator(ITokenDiscriminator tokenDiscriminator)
        {
            TokenDiscriminator = tokenDiscriminator;
        }

        /// <summary>
        /// Метод, проверяющий валидность формулы
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public bool Validate(ServerFormula formula)
        {
            if (string.IsNullOrWhiteSpace(formula?.FormulaString))
                return false;

            var tokens = TokenDiscriminator.ConstructTokens(formula.FormulaString);
            if ((tokens?.Count ?? 0) == 0 || tokens.Any(t => t is UnknownToken))
                return false;

            if (tokens.Any(token => token.Value == formula.Name)) //проверка на рекурсивную ссылку
                return false;

            var bracketsIsValid = ValidateBrackets(tokens);
            if (!bracketsIsValid)
                return false;

            var tokenSequenceIsValid = ValidateTokenSequence(tokens);
            if (!tokenSequenceIsValid)
                return false;

            return true;
        }

        /// <summary>
        /// Валидация скобок
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private bool ValidateBrackets(TokenCollection tokens)
        {
            var brackets = tokens.Where(t => t is BracketToken).Cast<BracketToken>().ToList();
            int bracketState = 0;

            foreach (var bracket in brackets)
            {
                if (bracket.IsOpening)
                    bracketState++;
                else
                    bracketState--;
                if (bracketState < 0)
                    return false;
            }

            return bracketState == 0;
        }

        /// <summary>
        /// Валидация последовательности элементов
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private bool ValidateTokenSequence(TokenCollection tokens)
        {
            bool isValid = true;
            var sequenceValidator = new TokenSequenceValidator();

            for (int i = 0; i < tokens.Count; i++)
            {
                (string, string) tokenTypeTuple;
                if (i == 0)
                    tokenTypeTuple = (TokenSequenceValidator.first, tokens[i].Name);
                else
                    tokenTypeTuple = (tokens[i - 1].Name, tokens[i].Name);

                isValid = sequenceValidator.Validate(tokenTypeTuple);
                if (!isValid)
                    return isValid;
            }
            return sequenceValidator.Validate((TokenSequenceValidator.last, tokens.Last().Name));
        }
    }
}
