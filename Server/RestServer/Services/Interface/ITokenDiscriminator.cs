using RestServer.Models.Tokens;

namespace RestServer.Services.Interface
{
    /// <summary>
    /// Разделитель формул на токены
    /// </summary>
    public interface ITokenDiscriminator
    {
        /// <summary>
        /// Последнее расчитанное значение
        /// </summary>
        TokenCollection Tokens { get; set; }
        /// <summary>
        /// Конструктор коллекции токенов
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>

        TokenCollection ConstructTokens(string exp);
    }
}