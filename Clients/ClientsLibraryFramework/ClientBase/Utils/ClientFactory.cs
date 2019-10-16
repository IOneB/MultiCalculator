using ClientsLibrary.ClientBase;
using ClientsLibrary.Serialization;
using System;
using System.Collections.Generic;

namespace ClientsLibrary
{
    /// <summary>
    /// Фабрика клиентов. Кэширует уже созданных клиентов.
    /// </summary>
    public class ClientFactory : IDisposable
    {
        //Коллекция созданных клиентов
        readonly Dictionary<string, ICalculatorClient> clients = new Dictionary<string, ICalculatorClient>();

        /// <summary>
        /// Адрес сервера для которого создаются клиенты
        /// </summary>
        public string ServerAddress { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverUri">Адрес сервера, придерживающийся протокола http</param>
        public ClientFactory(Uri serverUri)
        {
            if (serverUri is null || serverUri.Scheme != Uri.UriSchemeHttp)
            {
                throw new ArgumentException("Некорректный адрес сервера", (nameof(serverUri)));
            }

            this.ServerAddress = serverUri.ToString();
        }

        /// <summary>
        /// Фабричный метод создания клиентов
        /// </summary>
        /// <typeparam name="T">Тип, реализующий ICalculatorClient</typeparam>
        /// <param name="serializer">Сериализатор для задания значения клиенту</param>
        /// <returns></returns>
        public ICalculatorClient GetClient<T>(ISerializer serializer = null) where T : ICalculatorClient, new()
        {
            string key = typeof(T).FullName;
            if (!clients.ContainsKey(key))
                clients.Add(key, new T() { ServerAddress = ServerAddress}); // Создаем новый элемент, если такой клиент еще не был создан
            if (serializer != null && clients[key] is ISerializing serializing) // На случай если клиенту нужен новый сериализатор
                serializing.Serializer = serializer;
            return clients[key];
        }

        /// <summary>
        /// Освобождает все занятые ресурсы
        /// </summary>
        public void Dispose()
        {
            foreach (var item in clients)
                if (item.Value is IDisposable disposable)
                    disposable.Dispose();
        }
    }
}