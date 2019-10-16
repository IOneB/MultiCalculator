using ClientsLibrary.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientsLibrary
{
    /// <summary>
    /// Интерфейс клиентов калькулятора
    /// </summary>
    public interface ICalculatorClient : IDisposable
    {
        /// <summary>
        /// Адрес сервера
        /// </summary>
        string ServerAddress { get; set; }

        /// <summary>
        /// Запрос, которым клиент штурмует сервер
        /// </summary>
        /// <param name="parameters">Словарь параметров запроса</param>
        /// <returns></returns>
        Task<Maybe> RequestAsync<T>(Dictionary<string, string> parameters = null);
    }
}