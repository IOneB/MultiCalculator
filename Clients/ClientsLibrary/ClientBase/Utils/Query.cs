using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ClientsLibrary.ClientBase.Utils
{
    /// <summary>
    /// Структура запроса
    /// </summary>
    public struct Query
    {
        public Query(string address, HttpContent content)
        {
            Address = address;
            Content = content;
        }
        /// <summary>
        /// Деконструктор запроса для изменения или задания параметров
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Query With(HttpContent content) => new Query(Address, content);
        /// <summary>
        /// Адрес запроса
        /// </summary>
        public string Address { get; }
        /// <summary>
        /// Параметры запроса
        /// </summary>
        public HttpContent Content { get; }
    }
}
