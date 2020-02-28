namespace SportEvents.API.Sports
{
    public class SportsDTO
    {
        /// <summary>
        /// Идентификатор спорта
        /// </summary>
        public int SportId { get; set; }

        /// <summary>
        /// Наименование спорта
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Количество событий по этому спорту
        /// </summary>
        public int EventsCount { get; set; }
    }
}
