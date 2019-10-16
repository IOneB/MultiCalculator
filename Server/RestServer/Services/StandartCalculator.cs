using CommonLibrary.Model;
using RestServer.Models;
using RestServer.Models.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestServer.Services
{
    /// <summary>
    /// Конкретная реализация калькулятора формул
    /// </summary>
    class StandartCalculator : IFormulaCalculator
    {
        /// <summary>
        /// Асинхронный метод расчета формул
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public async Task CalculateAsync(ServerFormula formula, Func<Func<ServerFormula, bool>, Task<IEnumerable<ServerFormula>>> getSource)
        {
            bool isCalculated = false;
            DateTime startCalculateTime = DateTime.UtcNow;


            var tree = CreateTree(formula);

            // Если не будут получены все данные для расчета формулы в течение 10 минут, 
            // то формула считается невалидной и не будет расчитана 
            while (!isCalculated && (DateTime.UtcNow - startCalculateTime).TotalMinutes < 10)
            {
                isCalculated = await CalculateValue(tree, getSource);
                await Task.Delay(500);
            }

            if (!isCalculated)
                formula.Status = FormulaStatus.Error;
        }

        /// <summary>
        /// Построение дерева выражения
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        private object CreateTree(ServerFormula formula)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Расчет значения выражения формулы
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="getSource"></param>
        /// <returns></returns>
        private async Task<bool> CalculateValue(object tree, Func<Func<ServerFormula, bool>, Task<IEnumerable<ServerFormula>>> getSource)
        {
            // Если все участники определены, то можно приступать к расчету
            if ((await getSource(null)).Count() == (int)tree)
            {

                return true;
            }
            return false;
        }











        private static string BuildParseTree(string exp)
        {
            var stack = new Stack<Token>();
            TokenCollection tokens = GetTokens(exp);
            tokens.CreateRPN();
            return tokens.Calculate();
        }

        private static TokenCollection GetTokens(string exp)
        {
            var fplist = exp.Replace("(", " ( ").Replace(")", " ) ");
            var stringTokens = Operator.GetAll
                .Aggregate(fplist, (list, op) => list.Replace(op.Name, $" {op.Name} "))
                .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var tokens = new TokenCollection();
            tokens.AddMany(stringTokens);

            return tokens;
        }
    }
}
