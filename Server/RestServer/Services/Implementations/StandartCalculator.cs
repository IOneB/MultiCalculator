using RestServer.Models;
using RestServer.Services.Interface;
using System;
using System.Collections.Generic;

namespace RestServer.Services.Implementations
{
    /// <summary>
    /// Конкретная реализация калькулятора формул
    /// </summary>
    public class StandartCalculator : IFormulaCalculator
    {
        public ITokenDiscriminator TokenDiscriminator { get; }

        public StandartCalculator(ITokenDiscriminator tokenDiscriminator)
        {
            TokenDiscriminator = tokenDiscriminator;
        }

        /// <summary>
        /// Расчет значения формулы
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public bool Calculate(ServerFormula formula, List<ServerFormula> requirments)
        {
            var tokens = TokenDiscriminator.Tokens ?? TokenDiscriminator.ConstructTokens(formula.FormulaString);
            tokens.CreateRPN();
            try
            {
                formula.Value = tokens.Calculate(requirments);
            }
            catch(DivideByZeroException) { throw; }
            catch { return false; }
            return true;
        }
    }
}
