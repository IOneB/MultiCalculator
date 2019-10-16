using RestServer.Models;
using System;
using System.Collections.Generic;

namespace RestServer.Services
{
    /// <summary>
    /// Класс, реализующий валидацию формулы на стороне сервера
    /// </summary>
    class StandartValidator : IFormulaValidator
    {
        /// <summary>
        /// Метод, проверяющий валидность формулы
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public bool IsValid(ServerFormula formula)
        {
            throw new NotImplementedException();
        }

        public void SetInvalidNames(IEnumerable<string> names)
        {
            throw new NotImplementedException();
        }
    }
}
