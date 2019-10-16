using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RestServer.Models;
using RestServer.Services.Interface;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestServer.Hubs
{
    /// <summary>
    /// Хаб, реализующий соединение в режиме реального времени с клиентами
    /// </summary>
    [HubName("Formula")]
    public class FormulaHub : Hub<IFormulaHub>
    {
        static ConcurrentDictionary<string, List<string>> connectionGroups;
        Container container;


        public FormulaHub(Container _container)
        {
            connectionGroups = new ConcurrentDictionary<string, List<string>>();
            container = _container;
        }

        /// <summary>
        /// Асинхронный метод пересылки состояния. Добавляет пользователя в группу рассылки
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task GetState(string name)
        {
            //достаем формулу из репозитория, чтобы убедиться, что такая вообще есть
            ServerFormula formula = null;
            using (AsyncScopedLifestyle.BeginScope(container))
                formula = await container.GetInstance<IRepository<ServerFormula>>().GetByNameAsync(name); 

            if (formula is null)
                Clients.Caller.SendError(name, "Формула не найдена или была окончательно удалена", DateTime.UtcNow);
            else
            {
                //Добавляем в группу коннекшн и отправляем ему текущее состояние
                await Groups.Add(Context.ConnectionId, name);
                connectionGroups.AddOrUpdate(Context.ConnectionId,
                                             new List<string> { name },
                                             (key, value) =>
                                             {
                                                 value.Add(name);
                                                 return value;
                                             });

                Clients.Caller.SendState(formula.Name, formula.Status, DateTime.UtcNow);
            }
        }

        /// <summary>
        /// При разрыве коннекшена, он удаляется из всех групп. 
        /// При этом удаление из группы происходит без ожидания, 
        /// так как пользователь мог уже отключиться и не будет отвечать
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            bool hasValue = connectionGroups.TryRemove(Context.ConnectionId, out List<string> userGroups);
            if (hasValue)
                foreach (var group in userGroups)
                    Groups.Remove(Context.ConnectionId, group);

            return Task.CompletedTask;
        }
    }
}