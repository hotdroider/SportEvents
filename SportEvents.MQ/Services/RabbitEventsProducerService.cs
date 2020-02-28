using RabbitMQ.Client;
using SportEvents.MQ.Messages;
using System.Collections.Generic;

namespace SportEvents.MQ
{
    /// <summary>
    /// Поставщик событий в очердь
    /// </summary>
    public class RabbitEventsProducerService : RabbitServiceBase
    {
        public RabbitEventsProducerService(RabbitConfig config)
            : base(config) { }

        /// <summary>
        /// Толкнуть в очередь обновления по событиям
        /// </summary>
        /// <param name="updates"></param>
        public void PostUpdates(IEnumerable<EventMessage> updates)
        {
            foreach (var ev in updates)
            {
                var body = ev.ToMessageBody();
                Channel.BasicPublish(string.Empty, queueName, null, body);
            }
        }
    }
}
