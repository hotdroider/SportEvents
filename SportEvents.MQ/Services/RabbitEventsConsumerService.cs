using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SportEvents.MQ.Messages;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SportEvents.MQ
{
    /// <summary>
    /// Поедатель событий с очереди
    /// </summary>
    public class RabbitEventsConsumerService : RabbitServiceBase
    {
        public RabbitEventsConsumerService(RabbitConfig config)
            : base(config)
        {
            var consumer = new EventingBasicConsumer(Channel);

            consumer.Received += (model, ea) =>
            {
                var evnt = ea.Body.FromMessageBody<EventMessage>();

                _receivedPool.TryAdd(ea.DeliveryTag, evnt);
            };

            Channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);
        }

        /// <summary>
        /// Пул присланных сообщений
        /// </summary>
        private ConcurrentDictionary<ulong, EventMessage> _receivedPool = new ConcurrentDictionary<ulong, EventMessage>();

        /// <summary>
        /// Текущие накопления
        /// </summary>
        public List<(EventMessage msg, ulong tag)> GetCurrentBatch() => _receivedPool.Select(item => (item.Value, item.Key)).ToList();

        public void Acknoledge(IEnumerable<ulong> tags)
        {
            foreach (var tag in tags)
            {
                Channel.BasicAck(deliveryTag: tag, multiple: false);
                _receivedPool.TryRemove(tag, out _);
            }
        }
    }
}
