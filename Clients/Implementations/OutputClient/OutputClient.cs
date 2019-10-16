using ClientsLibrary;
using ClientsLibrary.ClientBase.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OutputClient
{
    /// <summary>
    /// Клиент, занимающийся получением конкретных значений формул
    /// </summary>
    public class OutputClient : TemplateMethodHttpCalculatorClient
    {
        protected override async Task<HttpResponseMessage> MakeRequestAsync(Query query) 
            => await HttpClient.GetAsync(query.Address, await query.Content.ReadAsStringAsync());

        protected override Query BuildQuery(Dictionary<string, string> parameters) 
            => base.BuildQuery(parameters)
                   .With(new StringContent($"?name={parameters["name"]}"));
    }
}
