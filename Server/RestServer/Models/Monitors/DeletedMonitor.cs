using CommonLibrary.Model;
using RestServer.Services.Interface;
using System.Threading.Tasks;

namespace RestServer.Models.Monitors
{
    class DeletedMonitor : MonitorStatusManager
    {
        public override async Task SearchAsync(IRepository<ServerFormula> repository, IFormulaCalculator formulaCalculator, IFormulaStatusManager statusManager)
        {
            this.repository = repository;
            this.formulaCalculator = formulaCalculator;
            this.statusManager = statusManager;

            var observerFormulas = await repository.GetAllAsync(f => f.Status == FormulaStatus.Deleted, 
                                                                tracking: true, 
                                                                withEncapsulated: true, 
                                                                withRequired: true);

            foreach (var formula in observerFormulas)
            {
                //Всех зависящих теперь нужно пересчитывать
                await ManageStatus(formula.Encapsulated, FormulaStatus.ErrorRequirements); 
                formula.Required.Clear(); //Избавляемся от зависимостией
                await repository.DeleteAsync(formula.FormulaId); //И можно спокойно удаляться
            }

            await repository.SaveAsync();
        }
    }
}
