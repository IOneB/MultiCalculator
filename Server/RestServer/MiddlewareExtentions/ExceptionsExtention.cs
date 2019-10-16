using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestServer.MiddlewareExtentions
{
    /// <summary>
    /// Расширение IAppBuilder, позволяющее добавить слой обработки исключений
    /// </summary>
    public static class ExceptionsExtention
    {
        /// <summary>
        /// Метод расширения, добавляющий слой обработки исключений
        /// </summary>
        /// <param name="appBuilder"></param>
        public static void UseExceptions(this IAppBuilder appBuilder)
        {
            appBuilder.Use(ExceptionMiddleware);
        }

        /// <summary>
        /// Непосредственно метод, занимающийся обработкой исключительных ситуаций для предоставления клиенту дополнительной информации
        /// </summary>
        /// <param name="context">Контекст</param>
        /// <param name="next">Следующий слой</param>
        /// <returns></returns>
        private static async Task ExceptionMiddleware(IOwinContext context, Func<Task> next)
        {
            await next();
        }
    }
}
