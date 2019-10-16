using Owin;
using System.Web.Http;
using System.Configuration;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using RestServer.MiddlewareExtentions;
using RestServer.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RestServer.Models;
using RestServer.Services.Implementations;
using RestServer.Services.Interface;
using RestServer.App_Data;

namespace RestServer
{
    public class Startup
    {
        /// <summary>
        /// Конфигурация REST-сервера
        /// </summary>
        /// <param name="appBuilder"></param>
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            RegisterServices(config);
            RegisterRoutes(config);
            RegisterMiddlewares(config, appBuilder);
        }

        private void RegisterServices(HttpConfiguration config)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            var scopedTransient = Lifestyle.CreateHybrid(Lifestyle.Scoped, Lifestyle.Transient);
            var scopedSingleton = Lifestyle.CreateHybrid(Lifestyle.Scoped, Lifestyle.Singleton);

            //Регистрация зависимостей
            container.Register<IFormulaCalculator, StandartCalculator>();
            container.Register<ILog, ConsoleLog>(scopedTransient);
            container.Register<CalculatorDbContext>(Lifestyle.Scoped);
            container.Register<IRepository<ServerFormula>, FormulaRepository>(scopedTransient);
            container.Register<IFormulaStatusManager, FormulaStatusManager>(scopedTransient);
            container.Register<IFormulaValidator, StandartValidator>(scopedTransient);
            container.Register<ITokenDiscriminator, StandartTokenDiscriminator>(scopedTransient);
            container.Register<IFormulaMonitor, StandartFormulaMonitor>(Lifestyle.Singleton);
            // Регистрируется кастомный активатор хабов для внедрения зависимостей в хабы
            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new CustomHubActivator(container));
            container.Verify();

            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }

        /// <summary>
        /// Определяются правила маршрутизации
        /// </summary>
        /// <param name="config"></param>
        private void RegisterRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultRoutes",
                routeTemplate: "api/{controller}/{name}",
                defaults: new { name = RouteParameter.Optional });
        }

        /// <summary>
        /// Слои сервера
        /// </summary>
        /// <param name="config"></param>
        /// <param name="appBuilder"></param>
        private void RegisterMiddlewares(HttpConfiguration config, IAppBuilder appBuilder)
        {
            if (ConfigurationManager.AppSettings["enableLogs"] == true.ToString())
                appBuilder.UseLogger();
            appBuilder.UseExceptions();
            appBuilder.MapSignalR("/FormulaState", new HubConfiguration());
            appBuilder.UseWebApi(config);
        }
    }
}
