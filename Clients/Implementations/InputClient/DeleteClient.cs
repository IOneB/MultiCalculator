using ClientsLibrary;
using ClientsLibrary.ClientBase.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InputClient
{
    /// <summary>
    /// Клиент, реализующий возможность удаления формул
    /// </summary>
    public class DeleteClient : TemplateMethodHttpCalculatorClient
    {
        /// <summary>
        /// Отправляет DELETE-запрос на сервер
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> MakeRequestAsync(Query query) 
            => await HttpClient.DeleteAsync(query.Address, await query.Content.ReadAsStringAsync());

        /// <summary>
        /// Строит запрос на основе параметров
        /// </summary>
        /// <param name="parameters">Параметры</param>
        /// <returns></returns>
        protected override Query BuildQuery(Dictionary<string, string> parameters = null)
            => base.BuildQuery(parameters).With(new StringContent($"?name={parameters["name"]}"));
    }
}
