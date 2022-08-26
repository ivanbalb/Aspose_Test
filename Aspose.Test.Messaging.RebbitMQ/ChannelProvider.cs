using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Aspose.Test.Messaging.RebbitMQ
{
    public interface IChannelProvider
    {
        IModel Channel { get; }
    }

    internal class ChannelProvider : IChannelProvider
    {
        private readonly ILogger<ChannelProvider> _logger;
        private readonly IConnection _connection;

        public IModel Channel { get; private set; }

        public ChannelProvider(ILogger<ChannelProvider> logger, IConfiguration config)
        {
            _logger = logger;

            var connectionString = config.GetRequiredSection("Messaging")["ConnectionString"];

            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri(connectionString);

            _connection = factory.CreateConnection();
            Channel = _connection.CreateModel();
            _logger.LogDebug("RabbitMQ channel opened");

            Channel.ModelShutdown += (sender, args) =>
            {
                _logger.LogDebug("RabitMQ channel closed : Initiator = {initiator} Cause = {reason}", args.Initiator, args.Cause);
                Channel.Close();
                Channel = _connection.CreateModel();
                _logger.LogDebug("RabbitMQ channel opened");
            };

            Channel.QueueDeclare(queue: new JobCreatedMessage(Guid.Empty).Channel,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            Channel.QueueDeclare(queue: new JobDownloadedMessage(Guid.Empty).Channel,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            Channel.QueueDeclare(queue: new JobParsedMessage(Guid.Empty).Channel,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        }
    }
}
