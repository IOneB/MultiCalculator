using System;
using System.Collections.Generic;
using System.Net.Http;
using ClientUI;
using CommonLibrary.Model;

namespace RealTimeClient
{
    class Program : ClientConsoleUI
    {
        private static Dictionary<string, UpdateInfo> observableFormulas;

        public static bool IsConnected { get; private set; } = false;

        static void Main()
        {
            observableFormulas = new Dictionary<string, UpdateInfo>();
            Console.WriteLine("Вас приветствует программа \"MultiCalculator RealTimeClient\"!\n" +
                  "Пожалуйста, введите название формулы для получения результата.\n" +
                  "Или введите \"!Exit\" для выхода");
            Console.WriteLine("Ожидаем подключения...");
            client = CreateClient<RealTimeClient>();
            if (client is RealTimeClient realTimeClient)
            {
                realTimeClient.OnReceive += Program_OnReceive;
                realTimeClient.OnReceiveError += Program_OnReceiveError;
                realTimeClient.Started += () =>
                {
                    IsConnected = true;
                    Console.WriteLine("Соединение установлено!");
                };
            }
            MainProcess();
            client.Dispose();
        }

        /// <summary>
        /// Создает поле, отражающее последнюю полученную информацию
        /// </summary>
        /// <param name="date"></param>
        private static void Construct(DateTime date)
        {
            Console.WriteLine($"Дата последнего обновления - {date}");
            Console.WriteLine("Пожалуйста, введите название формулы для наблюдения.\n" +
                  "Или введите \"!Exit\" для выхода");
            Console.WriteLine($"\tИмя\tСтатус\tВремя");
            foreach (var item in observableFormulas)
            {
                Console.WriteLine($"\t{item.Value.Name}" +
                                  $"\t{item.Value.Status?.GetDisplayName()}" +
                                  $"\t{item.Value.DateOfUpdate}");
            }
            Console.Write("Имя формулы для отслеживания - ");
        }

        private static void MainProcess()
        {
            var parameters = new Dictionary<string, string>() {["name"] = "!default" };
            while (true)
            {
                Console.Write("Имя формулы для отслеживания - ");
                var command = Console.ReadLine();

                if(string.IsNullOrWhiteSpace(command))
                {
                    Console.WriteLine("Введите значение");
                    continue;
                }

                if (command.ToLower() == "!exit")
                    break;

                if (!IsConnected)
                {
                    Console.WriteLine("Соединение еще не установлено!");
                    continue;
                }

                parameters["name"] = command;
                try
                {
                    client.RequestAsync<object>(parameters).Wait();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Cannot connect to server!");
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
        }

        private static void Program_OnReceiveError(string name, string error, DateTime date)
        {
            Console.WriteLine($"Ошибка \"{error}\" в получении значения формулы {name} ({date.ToString()})");
        }

        /// <summary>
        /// Получение состояния
        /// </summary>
        /// <param name="status"></param>
        private static void Program_OnReceive(string name, FormulaStatus status , DateTime date)
        {
            // Если клиент не следит за полученной формулой или ее последнее обновление было после этого, то нет смысла принимать изменения
            if (observableFormulas.TryGetValue(name, out UpdateInfo info) && date < info.DateOfUpdate) 
                return;
            observableFormulas[name] = new UpdateInfo(name, status, date);
            Console.Clear();
            Construct(date);
        }
    }
}
