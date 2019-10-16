namespace ClientsLibrary
{
    /// <summary>
    /// Функциональный паттерн. Возвращает либо значение, либо ошибку
    /// Позволяет избегать нагромождений исключений
    /// </summary>
    public abstract class Maybe
    {
        /// <summary>
        /// Типизированный результат
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Result<T> : Maybe
        {
            public T Value { get; set; }
            public Result(T value) => Value = value;
            public override string ToString() => Value?.ToString() ?? "Undefined";
        }
        /// <summary>
        /// Сущность ошибки
        /// </summary>
        public class Error : Maybe
        {
            public string Description { get; set; }
            public Error(string description) => Description = description;
            public override string ToString() => Description;
        }
    }
}