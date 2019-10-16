using CommonLibrary.Model;
using System.Collections.Generic;

namespace RestServer.Models
{
    /// <summary>
    /// Формула в ее представлении на сервере.
    /// Включает связи
    /// </summary>
    public class ServerFormula : Formula
    {
        /// <summary>
        /// Необходимые формулы
        /// </summary>
        public List<ServerFormula> Required { get; set; } = new List<ServerFormula>();
        /// <summary>
        /// Формулы, для которых необходима текущая
        /// </summary>
        public List<ServerFormula> Encapsulated { get; set; } = new List<ServerFormula>();
    }
}
