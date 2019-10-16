using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Model
{
    /// <summary>
    /// Статус расчета формулы
    /// </summary>
    public enum FormulaStatus
    {
        [Display(Name = "Ожидает расчета")]
        Waiting = 0,
        [Display(Name = "Идет расчет")]
        InProgress = 1,
        [Display(Name = "Завершено")]
        Success = 2,
        [Display(Name = "Ошибка расчета")]
        Error = 3,
        [Display(Name = "Ошибка деления на ноль")]
        DivideByZeroError = 4,
        [Display(Name = "Ошибка валидации")]
        ValidationError = 5,
        [Display(Name = "Ошибка. Не хватает одного или более значений")]
        ErrorRequirements = 6,
        [Display(Name = "Удалено")]
        Deleted = 7,
        [Display(Name = "Обновлено. Требуется пересчет")]
        Updated = 8,

    }
}