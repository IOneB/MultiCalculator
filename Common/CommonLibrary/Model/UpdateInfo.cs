using System;

namespace CommonLibrary.Model
{
    /// <summary>
    /// Структра для представления единицы обмена обновлением
    /// </summary>
    public struct UpdateInfo
    {
        /// <summary>
        /// Имя формулы, у которой обновился статус
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Новый статус
        /// </summary>
        public FormulaStatus? Status { get; }
        /// <summary>
        /// Дата обновления
        /// </summary>
        public DateTime DateOfUpdate { get; }
        /// <summary>
        /// Заполняется в случае ошибки
        /// </summary>
        public string Error { get; } 

        public UpdateInfo(string name, FormulaStatus newStatus, DateTime date = default)
        {
            if (date == default)
                DateOfUpdate = DateTime.UtcNow;
            else
                DateOfUpdate = date;
            Name = name;
            Status = newStatus;
            Error = null;
        }

        public UpdateInfo(string name, string error, DateTime date = default)
        {
            if (date == default)
                DateOfUpdate = DateTime.UtcNow;
            else
                DateOfUpdate = date;
            Name = name;
            Error = error;
            Status = null;
        }
    }
}
