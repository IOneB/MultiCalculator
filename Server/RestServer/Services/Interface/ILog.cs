namespace RestServer.Services.Interface
{
    /// <summary>
    /// Маленький кастомный интерфейс для логгера
    /// </summary>
    public interface ILog
    {
        void Info(string message);
        void Debug(string message);
        void Error(string message);
    }
}
