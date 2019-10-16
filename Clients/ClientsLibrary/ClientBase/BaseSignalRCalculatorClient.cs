using CommonLibrary;
using CommonLibrary.Model;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientsLibrary.ClientBase
{
    /// <summary>
    /// Базовый класс для клиентов хабов SignalR
    /// </summary>
    public abstract class BaseSignalRCalculatorClient : ICalculatorClient
    {
        /// <summary>
        /// Соединение с хабом на сервере
        /// </summary>
        protected HubConnection hubConnection;
        /// <summary>
        /// Прокси для работы с конкретным хабом
        /// </summary>
        protected IHubProxy proxy;
        /// <summary>
        /// Адрес сервера
        /// </summary>
        public string ServerAddress { get; set; }
        /// <summary>
        /// Адрес соединения с хабом
        /// </summary>
        public string ApiAddress {get;}

        public BaseSignalRCalculatorClient(string apiAddress)
        {
            ApiAddress = apiAddress;
            hubConnection = new HubConnection($"{CommonConfig.ServerConfig.FullAddress}/{ApiAddress}");
            ConfigureHubConnection();
        }

        /// <summary>
        /// Конфигурация соединения
        /// </summary>
        protected abstract Task ConfigureHubConnection();

        /// <summary>
        /// Запрос, которым клиент штурмует сервер
        /// </summary>
        /// <param name="parameters">Словарь параметров запроса</param>
        /// <returns></returns>
        public abstract Task<Maybe> RequestAsync<T>(Dictionary<string, string> parameters = null);

        /// <summary>
        /// Освобождает все ресурсы, включая соединение с хабом
        /// </summary>
        void IDisposable.Dispose()
        {
            hubConnection?.Stop();
            hubConnection?.Dispose();
        }
    }
}
