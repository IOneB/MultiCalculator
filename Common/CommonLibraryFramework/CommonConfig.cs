namespace CommonLibrary
{
    /// <summary>
    /// Класс, упрощающий работу с конфигурацией
    /// </summary>
    public class CommonConfig
    {
        /// <summary>
        /// Класс, представляющий настройки сервера
        /// </summary>
        public static ServerConfig ServerConfig { get; set; } = new ServerConfig();
    }
}
