using ClientsLibrary;
using ClientsLibrary.ClientBase;
using CommonLibrary;
using CommonLibrary.Model;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RealTimeClient
{
    /// <summary>
    /// Клиент, предназначенный для получения статуса расчета формула в реальном времени
    /// </summary>
    public class RealTimeClient : BaseSignalRCalculatorClient
    {
        /// <summary>
        /// Событие, вызываемое при принятии сообщения от хаба
        /// </summary>
        public event Action<string, FormulaStatus, DateTime> OnReceive;
        /// <summary>
        /// Событие, вызываемое при принятии ошибки от хаба
        /// </summary>
        public event Action<string, string, DateTime> OnReceiveError;
        /// <summary>
        /// Событие, вызываемое при подключении к хабу сервера
        /// </summary>
        public event Action Started;

        public RealTimeClient() : base("FormulaState") { }

        /// <summary>
        /// Конфигурация соединения
        /// </summary>
        protected override async Task ConfigureHubConnection()
        {
            //Событие потери соединения
            hubConnection.Closed += HubConnection_Closed;
            //Конфигурация прокси по определенному адресу
            proxy = hubConnection.CreateHubProxy("Formula");
            //Перехватываем получение ответа и вызываем событие
            proxy.On<string, FormulaStatus, DateTime>("SendState", (name, status, date) => OnReceive?.Invoke(name, status, date));
            proxy.On<string, string, DateTime>("SendError", (name, error, date) => OnReceiveError?.Invoke(name, error, date));

            await hubConnection.Start();
            Started?.Invoke();
        }

        /// <summary>
        /// При отключении от хаба пытается подключиться заново
        /// </summary>
        private async void HubConnection_Closed()
        {
            await Task.Delay(new Random().Next(3, 8) * 1000);
            try
            {
                Console.WriteLine("Попытка подключения...");
                await hubConnection.Start();
            }
            catch (HttpRequestException)
            {
                Console.WriteLine($"\tНевозможно соединиться с удаленным сервером {CommonConfig.ServerConfig.FullAddress}");
            }
        }

        /// <summary>
        /// Отсылает запрос на хаб с просьбой следить за конкретной формулой
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">Параметры запроса</param>
        /// <returns></returns>
        public override async Task<Maybe> RequestAsync<T>(Dictionary<string, string> parameters)
        {
            if (parameters.TryGetValue("name", out string value))
                await proxy.Invoke("GetState", value);
            return default; // Нет необходимости возвращать что-либо
        }
    }
}
