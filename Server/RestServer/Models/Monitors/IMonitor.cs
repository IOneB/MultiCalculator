using RestServer.Services.Interface;
using System.Threading.Tasks;

namespace RestServer.Models.Monitors
{
    /// <summary>
    /// Монитор это объект, следящий за каким-кто состоянием объектов
    /// и обрабатывающий эти объекты в рамках своей юрисдикции
    /// </summary>
    public interface IMonitor
    {
        Task SearchAsync(IRepository<ServerFormula> repository, IFormulaCalculator formulaCalculator, IFormulaStatusManager statusManage);
    }
}