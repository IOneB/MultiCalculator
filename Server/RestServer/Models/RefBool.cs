using System;

namespace RestServer.Models
{
    /// <summary>
    /// Способ обернуть логическое значение в ссылочный тип для передачи 
    /// состояния
    /// </summary>
    public class RefBool
    {
        public bool Value { get; set; }
        public RefBool(bool value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return obj is RefBool @bool &&
                   Value == @bool.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + Value.GetHashCode();
        }
    }
}