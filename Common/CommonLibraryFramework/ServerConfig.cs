using System.Configuration;

namespace CommonLibrary
{
    public class ServerConfig
    {
        /// <summary>
        /// IP-адрес/DNS сервера
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Порт
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Полный адрес сервера
        /// </summary>
        public string FullAddress => $"http://{Address}:{Port}";

        public ServerConfig()
        {
            Address = ConfigurationManager.AppSettings["ServerAddress"] ?? "127.0.0.1";
            Port = ConfigurationManager.AppSettings["ServerPort"] ?? "80";
        }

    }
}