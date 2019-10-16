namespace RestServer.Models
{
    /// <summary>
    /// Интерфейс объекта, способного оповещать о чем-либо
    /// </summary>
    public interface INotifier
    {
        void Notify();
    }
}