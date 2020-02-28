using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportEvents.MQ;
using SportEvents.Persistence;

namespace SportEvents.Importer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddPersistence(hostContext.Configuration);
                    services.AddMessageQueue(hostContext.Configuration);
                    services.AddHostedService<ImportService>();
                    services.AddHostedService<TestPublisherService>();
                });
    }
}
