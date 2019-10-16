using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Model
{
    /// <summary>
    /// Класс для представления сущности формулы
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Уникальный идентификатор формулы
        /// </summary>
        [Key]
        public int FormulaId { get; set; }
        /// <summary>
        /// Имя формулы
        /// </summary>
        [StringLength(20)]
        [CaseSensitive]
        public string Name { get; set; }
        /// <summary>
        /// Статус расчета формулы
        /// </summary>
        public FormulaStatus Status { get; set; }
        /// <summary>
        /// Результат
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        /// Содержание
        /// </summary>
        [Required]
        public string FormulaString { get; set; }

        public override string ToString()
        {
            string result = Name + ": ";
            if (Status == FormulaStatus.Success)
                result += Value.ToString();
            else
                result += $"{Status.GetDisplayName()}. Текущее значение ({Value.ToString()})";
            return result;
        }
    }
}