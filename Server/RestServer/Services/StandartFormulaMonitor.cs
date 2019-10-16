using System;
using System.Collections.Generic;
using System.Threading;
using RestServer.Models;
using SimpleInjector;

namespace RestServer.Services
{
    /// <summary>
    /// Реализация класса, следящего за процессом расчета формул
    /// </summary>
    public class StandartFormulaMonitor : IFormulaMonitor, IDisposable
    {
        private readonly Container _container;
        private readonly Thread _observe;
        public List<ServerFormula> ObservableFormulas { get; }

        public StandartFormulaMonitor(Container container)
        {
            ObservableFormulas = new List<ServerFormula>();
            this._container = container;
            _observe = new Thread(ObserveThread) { Name = nameof(StandartFormulaMonitor), IsBackground = true };
            _observe.Start();
        }

        public void ObserveThread()
        {
            while (true)
            {
                //if (condition)
                //{
                //    using (AsyncScopedLifestyle.BeginScope(_container))
                //        _container.GetInstance<IRepository<Formula>>().Create(null);
                //}
                Thread.Sleep(500);
            }
        }

        public void Dispose()
        {
            _observe.Abort();
        }
    }
}