namespace RestServer.Models
{
    /// <summary>
    /// Просто интерфейс, представляющий объект, способный проводить валидацию
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Метод валидации
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Validate(T entity);
    }
}