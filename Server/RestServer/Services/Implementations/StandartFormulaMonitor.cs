using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CommonLibrary.Model;
using RestServer.Models;
using RestServer.Models.Monitors;
using RestServer.Models.Monitors.Activators;
using RestServer.Services.Interface;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace RestServer.Services.Implementations
{
    /// <summary>
    /// Реализация класса, следящего за процессом расчета формул
    /// </summary>
    public class StandartFormulaMonitor : IFormulaMonitor
    {
        private readonly Container _container;
        // Активаторы мониторов
        private readonly MonitorActivator created = new MonitorActivator(200);
        private readonly MonitorActivator updated = new MonitorActivator(400);
        private readonly MonitorActivator deleted = new MonitorActivator(600);

        public IFormulaStatusManager StatusManager { get; }
        public IRepository<ServerFormula> Repository => _container.GetInstance<IRepository<ServerFormula>>();
        public IFormulaCalculator FormulaCalculator { get; }

        public StandartFormulaMonitor(Container container)
        {
            _container = container;
            StatusManager = _container.GetInstance<IFormulaStatusManager>();
            FormulaCalculator = _container.GetInstance<IFormulaCalculator>();

            ThreadPool.QueueUserWorkItem(Observe<CreatedMonitor>, created.IsActive);
            ThreadPool.QueueUserWorkItem(Observe<UpdatedMonitor>, updated.IsActive);
            ThreadPool.QueueUserWorkItem(Observe<DeletedMonitor>, deleted.IsActive);
        }

        /// <summary>
        /// Метод наблюдения
        /// Создается конкретный монитор и наблюдает.
        /// Каждый монитор обрабатывает данные в отдельном потоке
        /// </summary>
        /// <typeparam name="TMonitor"></typeparam>
        /// <param name="state"></param>
        public async void Observe<TMonitor>(object state) where TMonitor : IMonitor, new()
        {
            TMonitor monitor = new TMonitor();

            while (true)
            {
                if (((RefBool)state).Value)
                {
                    using (AsyncScopedLifestyle.BeginScope(_container))
                        await monitor
                                .SearchAsync(Repository, FormulaCalculator, StatusManager)
                                .ConfigureAwait(false);
                    ((RefBool)state).Value = false;
                }
                Thread.Sleep(300);
            }
        }

        public void Created() => created.IsActive.Value = true;
        public void Updated() => updated.IsActive.Value = true;
        public void Deleted() => deleted.IsActive.Value = true;
    }
}