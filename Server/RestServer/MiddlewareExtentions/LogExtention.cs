using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestServer.MiddlewareExtentions
{
    /// <summary>
    /// Расширение IAppBuilder, позволяющее добавить слой логирования запросов
    /// </summary>
    public static class LogExtention
    {
        /// <summary>
        /// Метод расширения, добавляющий слой логирования запросов и ответов
        /// </summary>
        /// <param name="appBuilder"></param>
        public static void UseLogger(this IAppBuilder appBuilder)
        {
            appBuilder.Use(LoggerMiddleware);
        }

        /// <summary>
        /// Непосредственно метод, ответственный за логирование приходящих запросов
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private static async Task LoggerMiddleware(IOwinContext context, Func<Task> next)
        {
            var request = context.Request;
            string debugInfo = $"[{DateTime.Now.ToString()}]: Incoming request. \n\t" +
                $"User-agent: {request.Headers["User-Agent"]}, \n\t" +
                $"uri: {request.Uri}, Method: {request.Method}, \n\t";
            Debug.WriteLine(debugInfo);

            await next();

            debugInfo = $"Response status: {context.Response.StatusCode}";
            Debug.WriteLine(debugInfo);
        }
    }
}
