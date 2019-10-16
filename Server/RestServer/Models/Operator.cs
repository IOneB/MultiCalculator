using System.Collections.Generic;

namespace RestServer.Models
{
    /// <summary>
    /// Класс, представляющий оператор. Оператор имеет наименование и приоритет
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// Строковое представление оператора
        /// </summary>
        public string Name { get; }

        protected Operator(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new System.ArgumentException("Operator must have name", nameof(name));

            Name = name;
        }

        /// <summary>
        /// Статический метод, позволяющий получить приоритет операции
        /// </summary>
        /// <param name="val"></param>
        /// <returns>Чем больше число, тем выше приоритет</returns>
        public static int Priority(string val)
        {
            switch (val)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// Статическое свойство, генерирует список допустимых операторов
        /// </summary>
        public static List<Operator> GetAll => new List<Operator>
        {
            new Operator("+"),
            new Operator("-"),
            new Operator("*"),
            new Operator("/")
        };
    }
}