using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestServer.Services
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
        Task<TEntity> GetById(int id);

        /// <summary>
        /// Найти элемент по имени
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByName(string name);

        /// <summary>
        /// Получить все элементы
        /// </summary>
        /// <param name="condition">условие фильтрации</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAll(Func<TEntity, bool> condition = null);

        /// <summary>
        /// Создать элемент
        /// </summary>
        /// <param name="entity"></param>
        Task Create(TEntity entity);

        /// <summary>
        /// Обновить элемент
        /// </summary>
        /// <param name="formula"></param>
        Task Update(TEntity formula);

        /// <summary>
        /// Удалить элемент по id
        /// </summary>
        /// <param name="id"></param>
        Task Delete(int id);

    }
}