using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Model;
using RestServer.Services.Interface;

namespace RestServer.Models.Monitors
{
    /// <summary>
    /// Слой для мониторов, добавляем возможность изменять статусы формул с оповещением
    /// </summary>
    abstract class MonitorStatusManager : IMonitor
    {
        protected IRepository<ServerFormula> repository;
        protected IFormulaCalculator formulaCalculator;
        protected IFormulaStatusManager statusManager;

        public async Task ManageStatus(IEnumerable<ServerFormula> formulas, FormulaStatus status)
        {
            foreach (var formula in formulas)
                statusManager
                    .ChangeStatus(formula, status)
                    .Notify();
            await repository.SaveAsync();
        }

        public abstract Task SearchAsync(IRepository<ServerFormula> repository, IFormulaCalculator formulaCalculator, IFormulaStatusManager statusManage);
    }
}
