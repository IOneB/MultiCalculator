using CommonLibrary.Model;
using RestServer.Models;

namespace RestServer.Services.Interface
{
    /// <summary>
    /// Интерфейс объекта, изменяющего статус формулы и готовящего объект, производящий оповещение всех нуждающихся
    /// </summary>
    public interface IFormulaStatusManager : INotifier
    {
        /// <summary>
        /// Изменяет статус формулы и представляет возможность для оповещения
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        INotifier ChangeStatus(ServerFormula formula, FormulaStatus newStatus);
    }
}
