using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Model
{
    /// <summary>
    /// Атрибует позвляет указать серверу как обрабатывать строку в плане чувствительности к регистру
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CaseSensitiveAttribute : Attribute
    {
        public bool IsEnabled { get; set; }

        public CaseSensitiveAttribute()
        {
            IsEnabled = true;
        }
    }
}
