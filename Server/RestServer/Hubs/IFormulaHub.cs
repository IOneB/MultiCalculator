using CommonLibrary.Model;
using System;
using System.Threading.Tasks;

namespace RestServer.Hubs
{
    /// <summary>
    /// Основной интерфейс хаба для отправки статуса расчета формулы или ошибки
    /// </summary>
    public interface IFormulaHub
    {
        void SendState(string name, FormulaStatus status, DateTime dateOfChange);
        void SendError(string name, string error, DateTime dateOfChange);
    }
}
