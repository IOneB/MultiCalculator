using RestServer.Models;

namespace RestServer.Services.Interface
{
    /// <summary>
    /// Интерфейс, представляющий валидацию формулы на стороне сервера
    /// </summary>
    public interface IFormulaValidator : IValidator<ServerFormula>
    {
        ITokenDiscriminator TokenDiscriminator { get; }
    }
}
