using System.Threading.Tasks;
using System.Collections.Generic;
using ClientsLibrary.ClientBase.Utils;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace InputClient
{
    /// <summary>
    /// Клиент, занимающийся созданием новых формул
    /// </summary>
    public class InputClient : TemplateMethodHttpCalculatorClient
    {
        public InputClient()
        {
            requiredParameters.Add("formulaString");
        }

        /// <summary>
        /// Отправляет POST-запрос на сервер с формулой для создания
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> MakeRequestAsync(Query query) => await HttpClient.PostAsync(query.Address, query.Content);

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
