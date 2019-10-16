using RestServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestServer.Services.Interface
{
    /// <summary>
    /// Интерфейс расчета значения формул
    /// </summary>
    public interface IFormulaCalculator
    {
        /// <summary>
        /// Разделитель формул на токены
        /// </summary>
        ITokenDiscriminator TokenDiscriminator { get; }

        /// <summary>
        /// Расчет значения формулы
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="requirments"></param>
        /// <returns></returns>
        bool Calculate(ServerFormula formula, List<ServerFormula> requirments);
    }
}