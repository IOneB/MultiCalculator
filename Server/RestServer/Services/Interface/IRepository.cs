using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RestServer.Services.Interface
{
    /// <summary>
    /// Интерфейс репозитория, обеспечивающий взаимодействие с базой данных
    /// </summary>
    public interface IRepository<TEntity>
    { 
        /// <summary>
        /// Найти элемент по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Найти элемент по имени
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByNameAsync(string name);


        /// <summary>
        /// Получить все элементы
        /// </summary>
        /// <param name="condition">условие фильтрации</param>
        /// <param name="tracking">Флаг отслеживания контекста</param>
        /// <param name="withRequired">Флаг загрузки зависимостей</param>
        /// <param name="withEncapsulated">Флаг загрузки зависимых</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> condition = null, bool tracking = false, bool withRequired = false, bool withEncapsulated = false);
        Task DeleteAsync(int formulaId);

        /// <summary>
        /// Создать элемент
        /// </summary>
        /// <param name="entity"></param>
        Task<bool> CreateAsync(TEntity entity);

        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="entity"></param>
        Task<bool> UpdateAsync(TEntity entity);

        /// <summary>
        /// Удалить элемент
        /// </summary>
        /// <param name="id"></param>
        Task<bool> DeleteAsync(TEntity entity);
        /// <summary>
        /// Асинхронный метод сохранения
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
    }
}