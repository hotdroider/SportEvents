using RabbitMQ.Client;
using System;

namespace SportEvents.MQ
{
    /// <summary>
    /// Просто канал на очередь
    /// </summary>
    public abstract class RabbitServiceBase : IDisposable
    {
        ConnectionFactory _connectionFactory;
        IConnection _connection;
        protected string queueName;

        public IModel Channel { get; }

        public RabbitServiceBase(RabbitConfig config)
        {
            //сетапимся с конфигов
            _connectionFactory = new ConnectionFactory()
            {
                HostName = config.HostName,
                Port = config.Port,
                UserName = config.UserName,
                Password = config.Password
            };

            queueName = config.QueueName;

            //открываем канал
            _connection = _connectionFactory.CreateConnection();
            Channel = _connection.CreateModel();

            Channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void Dispose()
        {
            Channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
