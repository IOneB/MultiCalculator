using RestServer.Models;
using System.Collections.Generic;

namespace RestServer.Services
{
    /// <summary>
    /// Интерфейс, представляющий валидацию формулы на уровне сервера
    /// </summary>
    public interface IFormulaValidator
    {
        /// <summary>
        /// Метод валидации, возвращающий логическое значение, характеризующее правильность составления формулы
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        bool IsValid(ServerFormula formula);
        void SetInvalidNames(IEnumerable<string> names);
    }
}
