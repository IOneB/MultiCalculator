using RestServer.Models;
using System.Collections.Generic;

namespace RestServer.Services
{
    /// <summary>
    /// Интерфейс, отслеживающий состояние расчета формул, которые еще не попали в базу
    /// </summary>
    public interface IFormulaMonitor
    {
        List<ServerFormula> ObservableFormulas { get; }
    }
}