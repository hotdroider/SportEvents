using SportEvents.Core.Enums;
using System;

namespace SportEvents.Core.Entities
{
    /// <summary>
    /// Спортивное событие
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Идентифиактор события
        /// </summary>
        public long EventId { get; set; }

        /// <summary>
        /// Идентифиактор спорта
        /// </summary>
        public int SportId { get; set; }

        /// <summary>
        /// Спорт
        /// </summary>
        public Sport Sport { get; set; }

        /// <summary>
        /// Название события
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата события
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Статус события
        /// </summary>
        public EventStatus Status { get; set; }

        /// <summary>
        /// Коэффициент на победу 1й команды
        /// </summary>
        public decimal Team1Price { get; set; }

        /// <summary>
        /// Коэффициент на ничью
        /// </summary>
        public decimal DrawPrice { get; set; }

        /// <summary>
        /// Коэффициент на победу 2й команды
        /// </summary>
        public decimal Team2Price { get; set; }
    }
}
