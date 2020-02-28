using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SportEvents.MQ;
using SportEvents.MQ.Messages;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportEvents.Importer
{
    /// <summary>
    /// Тестовый поставщик обновлений в очередь
    /// </summary>
    class TestPublisherService : BackgroundService
    {
        private readonly ILogger<ImportService> _logger;

        private readonly RabbitEventsProducerService _mqService;

        public TestPublisherService(ILogger<ImportService> logger, RabbitEventsProducerService mqService)
        {
            _logger = logger;
            _mqService = mqService;
        }

        private Random rnd = new Random();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //idle for some time
                    await Task.Delay(rnd.Next(1, 10) * 1000, stoppingToken);

                    //push some evs
                    var evs = rnd.Next(10);

                    _logger.LogInformation($"{DateTime.Now.ToShortTimeString()}, Pushing {evs} events...");

                    var ups = Enumerable.Range(0, evs).Select(i => new EventMessage()
                    {
                        TimeStamp = DateTime.Now.Ticks,
                        SportId = rnd.Next(1, 4),
                        EventId = rnd.Next(20),
                        Name = "Random Event",
                        Status = (Core.Enums.EventStatus)rnd.Next(3),
                        EventDate = DateTime.Today.AddDays(rnd.Next(5)),
                        Team1Price = new decimal(rnd.NextDouble()),
                        Team2Price = new decimal(rnd.NextDouble()),
                        DrawPrice = new decimal(rnd.NextDouble())
                    });

                    _mqService.PostUpdates(ups);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Error pushing events to queue");
                }
            }
        }
    }
}
