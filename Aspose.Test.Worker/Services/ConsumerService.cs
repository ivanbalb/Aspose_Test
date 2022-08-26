using Aspose.Test.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aspose.Test.Worker.Services
{
    public abstract class ConsumerService<TMessage> : IHostedService where TMessage : Message
    {
        private readonly ILogger<ConsumerService<TMessage>> _logger;
        private readonly IConsumer<TMessage> _consumer;

        public ConsumerService(ILogger<ConsumerService<TMessage>> logger,
            IConsumer<TMessage> consumer)
        {
            _logger = logger;
            _consumer = consumer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting {service}", this.GetType().Name);
            await _consumer.SubscribeAsync(HandleMessage);
            _logger.LogDebug("{service} started", this.GetType().Name);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("{service} stoped", this.GetType().Name);
            return Task.CompletedTask;
        }

        protected abstract Task HandleMessage(TMessage message);
    }
}
