using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CommonLibrary.Model
{
    /// <summary>
    /// Расширение статуса, переводящее его в читаемый вид
    /// </summary>
    public static class StatusExtension
    {
        public static string GetDisplayName(this FormulaStatus status)
        {
            return status.GetType().GetMember(status.ToString())
                   .FirstOrDefault()?
                   .GetCustomAttribute<DisplayAttribute>()?
                   .Name ?? "";
        }
    }
}
