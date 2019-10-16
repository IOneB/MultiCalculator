using ClientsLibrary;
using ClientUI;
using CommonLibrary.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutputClient
{
    class Program : ClientConsoleUI
    {
        static void Main()
        {
            Console.WriteLine("Вас приветствует программа \"MultiCalculator OutpuClient\"!\n" +
                "Пожалуйста, введите название формулы для получения результата.\n" +
                "Или введите \"!Exit\" для выхода");

            client = CreateClient<OutputClient>();
            MainProcess();
        }

        /// <summary>
        /// Главный процесс приложения
        /// </summary>
        /// <returns></returns>
        private static void MainProcess()
        {
            while (true)
            {
                Console.Write("Имя формулы - ");
                var command = Console.ReadLine();

                if (command.ToLower() == "!exit")
                    break;

                if (string.IsNullOrWhiteSpace(command))
                    Console.WriteLine("Пожалуйста, введите корректное имя формулы");
                else
                {
                    parameters["name"] = command;
                    var response = client.RequestAsync<Formula>(parameters).Result;
                    Console.WriteLine(response);
                }
            }
            Console.WriteLine("До свидания!");
            Dispose();
        }
    }
}
