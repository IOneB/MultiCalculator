using RestServer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestServer.Services
{
    /// <summary>
    /// Интерфейс расчета значения формул
    /// </summary>
    public interface IFormulaCalculator
    {
        /// <summary>
        /// Асинхронный метод, занимающийся расчетом значения формул
        /// </summary>
        /// <param name="formula">формула</param>
        /// <param name="getSource">функция, возвращающая список элементов с учетом фильтрации</param>
        /// <returns></returns>
        Task CalculateAsync(ServerFormula formula, Func<Func<ServerFormula, bool>, Task<IEnumerable<ServerFormula>>> getSource);
    }
}