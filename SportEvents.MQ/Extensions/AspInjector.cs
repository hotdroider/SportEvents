using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SportEvents.MQ
{
    public static class AspInjector
    {
        public static IServiceCollection AddMessageQueue(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitConfigSection = configuration.GetSection("RabbitConfig");
            if (rabbitConfigSection == null)
                throw new ApplicationException("Rabbit config missing in config file");
     
            services.AddSingleton(rabbitConfigSection.Get<RabbitConfig>());
            services.AddTransient<RabbitEventsConsumerService>();
            services.AddTransient<RabbitEventsProducerService>();

            return services;
        }
    }
}
