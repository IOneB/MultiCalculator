using ClientsLibrary.Serialization;

namespace ClientsLibrary.ClientBase
{
    /// <summary>
    /// Интерфейс, представляющий объект, которому нужна возможность сериализации
    /// </summary>
    public interface ISerializing
    {
        /// <summary>
        /// Сериализатор результатов
        /// </summary>
        ISerializer Serializer { get; set; }
    }
}
