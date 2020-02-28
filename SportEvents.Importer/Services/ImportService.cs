using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SportEvents.Core.Entities;
using SportEvents.MQ;
using SportEvents.MQ.Messages;
using SportEvents.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportEvents.Importer
{
    public class ImportService : BackgroundService
    {
        private readonly ILogger<ImportService> _logger;

        private readonly RabbitEventsConsumerService _mqService;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ImportService(IServiceScopeFactory serviceScopeFactory,
            ILogger<ImportService> logger, 
            RabbitEventsConsumerService mqService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _mqService = mqService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Start import loop...");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10 * 1000, stoppingToken);

                var batchToWork = _mqService.GetCurrentBatch();

                //обновки по имеющимся событиям возьмем только самые новые, историчность вроде не нужна
                var newestUpdates = batchToWork
                    .Where(ev => ev.msg.EventId > 0)
                    .GroupBy(ev => ev.msg.EventId)
                    .Select(g => g.OrderBy(i => i.msg.TimeStamp).Last());

                //а эти надо вставить
                var insertUs = batchToWork.Where(ev => ev.msg.EventId == 0);

                var all = newestUpdates.Union(insertUs).ToList();

                _logger.LogInformation($"Process for {all.Count} messages, {batchToWork.Count - all.Count} skipped due to obsolition");

                all.AsParallel().ForAll(item => ProcessEvent(item.msg));

                _mqService.Acknoledge(batchToWork.Select(i => i.tag));
            }
        }

        bool ProcessEvent(EventMessage eventMessage)
        {
            try
            {
                _logger.LogInformation($"Received update for {eventMessage} as {DateTime.Now}");

                using var scope = _serviceScopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<SportEventsDbContext>();

                var eventToUpdate = dbContext.Events.FirstOrDefault(ev => ev.EventId == eventMessage.EventId);
                var needInsert = eventToUpdate == null;
                if (eventToUpdate == null)
                {
                    eventToUpdate = new Event();
                    dbContext.Add(eventToUpdate);
                }

                eventToUpdate.UpdateByMessage(eventMessage);

                dbContext.SaveChanges();

                _logger.LogInformation($"{(needInsert ? "Added" : "Updated")} event {eventToUpdate.EventId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exeption occured processing queue element");
                return false;
            }

            return true;
        }
    }
}
