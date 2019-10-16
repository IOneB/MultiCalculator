using ClientsLibrary.ClientBase.Utils;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace InputClient
{
    /// <summary>
    /// Клиент, занимающийся обновлением формул
    /// </summary>
    public class UpdateClient : TemplateMethodHttpCalculatorClient
    {
        public UpdateClient()
        {
            requiredParameters.Add("formulaString");
        }

        /// <summary>
        /// PUT-запрос на сервер, позволяющий изменить формулу
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> MakeRequestAsync(Query query) => await HttpClient.PutAsync(query.Address, query.Content);

        /// <summary>
        /// Строит JSON-тело запроса
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected override Query BuildQuery(Dictionary<string, string> parameters)
        {
            var content = new ObjectContent<Dictionary<string, string>>(parameters, new JsonMediaTypeFormatter());
            var query = base.BuildQuery(parameters);
            return query.With(content);
        }
    }
}
