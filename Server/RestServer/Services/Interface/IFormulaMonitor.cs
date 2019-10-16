using RestServer.Models;
using System;
using System.Collections.Generic;

namespace RestServer.Services.Interface
{
    /// <summary>
    /// Интерфейс, отслеживающий состояние расчета формул, которые еще не попали в базу
    /// </summary>
    public interface IFormulaMonitor
    {
        IRepository<ServerFormula> Repository { get; }
        IFormulaCalculator FormulaCalculator { get; }
        /// <summary>
        /// Позволяет оповестить нужный монитор о добавлении новой формулы
        /// </summary>
        void Created();
        /// <summary>
        /// Позволяет оповестить нужный монитор об изменении формулы
        /// </summary>
        void Updated();
        /// <summary>
        /// Позволяет оповестить нужный монитор об удалении формулы
        /// </summary>
        void Deleted();
    }
}