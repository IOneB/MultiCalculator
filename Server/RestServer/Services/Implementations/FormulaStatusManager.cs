using System;
using System.Threading;
using CommonLibrary.Model;
using Microsoft.AspNet.SignalR;
using RestServer.Hubs;
using RestServer.Models;
using RestServer.Services.Interface;

namespace RestServer.Services.Implementations
{
    /// <summary>
    /// Реализация менеджера статусов формул
    /// </summary>
    public class FormulaStatusManager : IFormulaStatusManager
    {
        UpdateInfo info;

        public INotifier ChangeStatus(ServerFormula formula, FormulaStatus newStatus)
        {
            if (formula is null)
                throw new ArgumentNullException(nameof(formula));

            formula.Status = newStatus;

            // Информация в обновлении
            info = new UpdateInfo(formula?.Name, newStatus);

            return this;
        }

        /// <summary>
        /// Оповестить всех подписавшихся через хаб, что состояние изменилось
        /// </summary>
        public void Notify()
        {
            ThreadPool.QueueUserWorkItem( _ =>
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<FormulaHub>();
                hubContext
                    .Clients
                    .Group(info.Name)
                    .SendState(info.Name, info.Status, info.DateOfUpdate);
            });
        }
    }
}
