using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientsLibrary.ClientBase.Utils
{
    /// <summary>
    /// Слой абстракции перед клиентами, представляющий реализацию паттерна шаблонный метод
    /// </summary>
    public abstract class TemplateMethodHttpCalculatorClient : BaseHttpCalculatorClient
    {
        /// <summary>
        /// Список строк. 
        /// Отвечает на вопрос какие параметры считать обязательными
        /// </summary>
        protected List<string> requiredParameters;

        protected TemplateMethodHttpCalculatorClient()
        {
            ApiUrl = "api/calculator/";
            requiredParameters = new List<string> { "name" };
        }

        public override async Task<Maybe> RequestAsync<T>(Dictionary<string, string> parameters)
        {
            Query query = BuildQuery(parameters); 

            HttpResponseMessage response;
            try
            {
                using (response = await MakeRequestAsync(query).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                        return await ParseSuccessAsync<T>(response);
                    else
                        return await ParseErrorAsync(response);
                }
            }
            catch
            {
                return new Maybe.Error("Произошла ошибка при соединении с сервером. Проверьте подключение");
            }
        }

        /// <summary>
        /// Этап на котором строится запрос для вызова действия сервера
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual Query BuildQuery(Dictionary<string, string> parameters)
        {
            foreach (var param in requiredParameters)
            {
                if (parameters is null || !parameters.ContainsKey(param) || string.IsNullOrEmpty(parameters[param]))
                    throw new ArgumentException($"Отсутствует необходимый параметр {param}");
            }

            Query query = new Query($"{ServerAddress}{ApiUrl}", null);
            return query;
        }

        /// <summary>
        /// Этап выполнения запроса
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected abstract Task<HttpResponseMessage> MakeRequestAsync(Query query);

        /// <summary>
        /// Этап парсинга в случае ошибки
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual async Task<Maybe> ParseErrorAsync(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                case System.Net.HttpStatusCode.Conflict:
                case System.Net.HttpStatusCode.InternalServerError:
                    return new Maybe.Error(await response.Content.ReadAsStringAsync());
                default: break;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new Maybe.Error("Формула не найдена");
            return new Maybe.Error(response.ReasonPhrase);
        }
        /// <summary>
        /// Этап парсинга успешного запроса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual async Task<Maybe> ParseSuccessAsync<T>(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                var targetObject = Serializer.Deserialize<T>(responseContent);
                return new Maybe.Result<T>(targetObject);
            }
            catch (Exception)
            {
                return new Maybe.Error($"Ошибка. Неизвестный формат ответа - {responseContent}");
            }
        }

    }
}
