using System;
using System.Threading;

namespace RestServer.Models.Monitors.Activators
{
    /// <summary>
    /// Активатор заставляет мониторы срабатывать как миниму один раз в установленное время по таймеру
    /// </summary>
    public class MonitorActivator : IDisposable
    {
        public RefBool IsActive { get; set; }
        public Timer Timer { get; }

        public MonitorActivator(int dueTime, bool isActive = false, int waitTime = 10_000)
        {
            IsActive = new RefBool(isActive);
            Timer = new Timer(s => IsActive.Value = true,
                      null,
                      dueTime,
                      waitTime);
        }

        public void Dispose()
        {
            Timer.Dispose();
        }
    }
}