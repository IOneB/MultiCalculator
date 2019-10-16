using CommonLibrary.Model;
using RestServer.Services.Interface;
using System.Threading.Tasks;

namespace RestServer.Models.Monitors
{
    /// <summary>
    /// Монитор, следящий за обновленными формулами
    /// </summary>
    class UpdatedMonitor : MonitorStatusManager
    {
        public override async Task SearchAsync(IRepository<ServerFormula> repository, IFormulaCalculator formulaCalculator, IFormulaStatusManager statusManager)
        {
            this.repository = repository;
            this.formulaCalculator = formulaCalculator;
            this.statusManager = statusManager;

            var observerFormulas = await repository.GetAllAsync(f => f.Status == FormulaStatus.Updated,
                                                                tracking: true,
                                                                withEncapsulated: true,
                                                                withRequired: true);

            foreach (var formula in observerFormulas)
            {
                //Говорим, что всех зависимых тоже нужно будем пересчитать
                await ManageStatus(formula.Encapsulated, FormulaStatus.Updated); 
                formula.Required.Clear();
            }

            await ManageStatus(observerFormulas, FormulaStatus.Waiting); // И ждем пересчета
            await repository.SaveAsync();
        }
    }
}
