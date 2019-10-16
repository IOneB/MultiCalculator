using System;
using System.Collections.Generic;
using System.Text;

namespace ClientsLibrary.Serialization
{
    /// <summary>
    /// Интерфейс объекта, занимающего сериализацией
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Преобразует объект в строковое представление
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        string Serialize<T>(T obj);
        /// <summary>
        /// Преобразует строку в объект
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        T Deserialize<T>(string value); 
    }
}
