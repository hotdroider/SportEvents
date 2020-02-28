using System.Collections.Generic;

namespace SportEvents.Core.Entities
{
    /// <summary>
    /// Вид спорта
    /// </summary>
    public class Sport
    {
        /// <summary>
        /// Идентифиактор
        /// </summary>
        public int SportId { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        public List<Event> Events { get; set; }
    }
}
