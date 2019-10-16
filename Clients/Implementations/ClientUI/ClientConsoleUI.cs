using ClientsLibrary;
using System;
using CommonLibrary;
using ClientsLibrary.Serialization;
using System.Collections.Generic;

namespace ClientUI
{
    /// <summary>
    /// Абстрактный класс, инкапсулирующий общую логику консольных приложений клиента калькулятора
    /// </summary>
    public abstract class ClientConsoleUI
    {
        static ClientFactory clientFactory;
        /// <summary>
        /// Словарь параметров для передачи в запросе клиентом
        /// </summary>
        protected static Dictionary<string, string> parameters;

        static ClientConsoleUI()
        {
            var serverUrl = new Uri(CommonConfig.ServerConfig.FullAddress);
            clientFactory = new ClientFactory(serverUrl);
            parameters = new Dictionary<string, string>
            {
                ["name"] = "default",
                ["formulaString"] = "default"
            };
        }

        /// <summary>
        /// Клиент приложения, отправляющий запрос серверу
        /// </summary>
        protected static ICalculatorClient client;
        /// <summary>
        /// Фабричный метод создания клиентов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">При желании можно переопределить стандартный механизм сериализации</param>
        /// <returns></returns>
        protected static ICalculatorClient CreateClient<T>(ISerializer serializer = null) where T: ICalculatorClient, new()
            => clientFactory.GetClient<T>(serializer);

        /// <summary>
        /// Возможность переподключиться к другому серверу
        /// </summary>
        /// <param name="serverAddress"></param>
        protected static void ChangeConnect(Uri serverAddress)
        {
            Dispose();
            clientFactory = new ClientFactory(serverAddress);
            client = null;
        }

        /// <summary>
        /// Освобождает все ресурсы
        /// </summary>
        public static void Dispose()
        {
            clientFactory.Dispose();
        }
    }
}
