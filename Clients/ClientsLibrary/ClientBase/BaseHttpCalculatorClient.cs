using ClientsLibrary.ClientBase;
using ClientsLibrary.Serialization;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClientsLibrary
{
    /// <summary>
    /// Базовый класс для http-клиентов калькулятора
    /// </summary>
    public abstract class BaseHttpCalculatorClient : ICalculatorClient, ISerializing
    {
        //Клиент для отправки запросов на сервер
        protected static readonly HttpClient HttpClient = new HttpClient();
        /// <summary>
        /// Адрес сервера
        /// </summary>
        public string ServerAddress { get; set; }
        /// <summary>
        /// Адрес запроса, которым оперирует клиент
        /// </summary>
        protected string ApiUrl { get; set; }

        /// <summary>
        /// Сериализатор результатов. По умолчанию - json
        /// </summary>
        public ISerializer Serializer { get; set; } = new JsonSerializer();

        public virtual void Dispose()
        {
            HttpClient.Dispose();
        }
        
        /// <summary>
        /// Запрос, которым клиент штурмует сервер
        /// </summary>
        /// <param name="parameters">Словарь параметров запроса</param>
        /// <returns></returns>
        public abstract Task<Maybe> RequestAsync<T>(Dictionary<string, string> parameters);
    }
}