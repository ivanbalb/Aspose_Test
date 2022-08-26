using Microsoft.Extensions.DependencyInjection;

namespace Aspose.Test.Messaging.RebbitMQ
{
    public static class Extensions
    {
        public static IServiceCollection AddRebbitMQMessaging(this IServiceCollection services)
        {
            services.AddSingleton<IChannelProvider, ChannelProvider>();

            services.AddScoped<IPublisher<JobMessage>, JobPublisher>();

            services.AddTransient<IConsumer<JobCreatedMessage>, JobConsumer<JobCreatedMessage>>();
            services.AddTransient<IConsumer<JobDownloadedMessage>, JobConsumer<JobDownloadedMessage>>();
            services.AddTransient<IConsumer<JobParsedMessage>, JobConsumer<JobParsedMessage>>();

            return services;
        }
    }
}
