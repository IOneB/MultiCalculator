using System;
using ClientUI;

namespace InputClient
{
    class Program : ClientConsoleUI
    {
        static void Main()
        {
            Console.WriteLine("Вас приветствует программа \"MultiCalculator InputClient\"!\n" +
                "Вы пользуетесь клиентом для ввода новых формул, а также изменения и удаления старых.\n" +
                "Для начала выберите команду, затем введите название формулы и после - содержание\n" +
                "Для выхода введите \"!Exit\"");
            MainProcess();
        }

        /// <summary>
        /// Основной рабочий процесс 
        /// </summary>
        /// <returns></returns>
        private static void MainProcess()
        {
            while (true)
            {
                Console.WriteLine("Введите 1 для ввода новой формулы, 2 - для обновления формулы, 3 - для удаления, !Exit - для выхода");
                var command = Console.ReadLine();

                if (command.ToLower() == "!exit")
                    break;

                switch (command)
                {
                    case "1":
                        client = CreateClient<InputClient>();
                        break;
                    case "2":
                        client = CreateClient<UpdateClient>();
                        break;
                    case "3":
                        client = CreateClient<DeleteClient>();
                        break;
                    default:
                        Console.WriteLine("Ошибка! Неизвестная команда");
                        continue;
                }

                if (!AskFormulaName())
                    continue;
                if (command != "3") // При удалении не нужно знать значение формулы
                    if (!AskFormulaValue())
                        continue;

                var response = client.RequestAsync<string>(parameters).Result;
                Console.WriteLine(response);
            }
            Console.WriteLine("До свидания!");
            Dispose();
        }

        private static bool AskFormulaName() => AskUser("Имя формулы - ", "name");

        private static bool AskFormulaValue() => AskUser($"Введите значение формулы\n{parameters["name"]} = ", "formulaString");

        /// <summary>
        /// Генератор метода, задающего вопрос и сохраняющего ответ
        /// </summary>
        /// <param name="question"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static bool AskUser(string question, string parameter)
        {
            Console.Write(question);
            string answer = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(answer))
            {
                Console.WriteLine("Значение не было получено");
                return false;
            }
            parameters[parameter] = answer;
            return true;
        }
    }
}
