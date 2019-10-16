using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestServer.Models.Tokens
{
    /// <summary>
    /// Интерфейс, представляющий токен, участвующий в итоговом расчете значения выражения
    /// </summary>
    public interface ITokenCalculator
    {
        /// <summary>
        /// Метод расчета итогового значения выражения
        /// </summary>
        /// <param name="stack">Промежуточный стек, содержащий текущее состояние</param>
        void Calculate(Stack<double> stack);
    }
}
