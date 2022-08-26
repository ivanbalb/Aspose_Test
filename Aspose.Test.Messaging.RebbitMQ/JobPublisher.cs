using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Aspose.Test.Messaging.RebbitMQ
{
    internal class JobPublisher : IPublisher<JobMessage>
    {
        private readonly ILogger<JobPublisher> _logger;
        private readonly IChannelProvider _provider;

        public JobPublisher(ILogger<JobPublisher> logger, IChannelProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public Task PublishAsync(JobMessage message)
        {
            var properties = _provider.Channel.CreateBasicProperties();
            properties.Persistent = true;

            _provider.Channel.BasicPublish(exchange: "",
                                 routingKey: message.Channel,
                                 basicProperties: properties,
                                 body: message.Id.ToByteArray());

            _logger.LogDebug("Publish job Id = {id} to {channel} channel", message.Id, message.Channel);

            return Task.CompletedTask;
        }
    }
}