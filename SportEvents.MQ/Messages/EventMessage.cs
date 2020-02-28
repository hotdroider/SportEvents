using SportEvents.Core.Enums;
using System;

namespace SportEvents.MQ.Messages
{
    /// <summary>
    /// Сообщение по обновлению события
    /// </summary>
    public class EventMessage
    {
        /// <summary>
        /// Временная метка
        /// </summary>
        public long TimeStamp { get; set; }

        /// <summary>
        /// Идентифиактор события
        /// </summary>
        public long EventId { get; set; }

        /// <summary>
        /// Идентифиактор спорта
        /// </summary>
        public int SportId { get; set; }

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
