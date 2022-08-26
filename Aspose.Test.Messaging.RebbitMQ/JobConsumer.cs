using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Messaging.RebbitMQ
{
    internal class JobConsumer<TMessage> : IConsumer<TMessage> where TMessage : JobMessage
    {
        private readonly ILogger<JobConsumer<TMessage>> _logger;
        private readonly IChannelProvider _provider;

        public JobConsumer(ILogger<JobConsumer<TMessage>> logger, IChannelProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public Task SubscribeAsync(Func<TMessage, Task> handler)
        {
            _provider.Channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var queueName = (Activator.CreateInstance(typeof(TMessage), Guid.Empty) as TMessage).Channel;

            var consumer = new EventingBasicConsumer(_provider.Channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();

                var message = Activator.CreateInstance(typeof(TMessage), new Guid(body)) as TMessage;
                _logger.LogDebug("Job {id} received from channel {channel}", message?.Id, queueName);

                var watch = new Stopwatch();
                watch.Start();
                try
                {
                    handler(message).Wait();
                    _provider.Channel.BasicAck(args.DeliveryTag, false);

                    _logger.LogDebug("Job {id} from channel {channel} processed ({time}ms}", message.Id, queueName, watch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Job {id} failed: {msg}", message.Id, ex.Message);

                    _provider.Channel.BasicNack(args.DeliveryTag, false, true);
                }
                finally
                {
                    watch.Stop();
                }
            };

            _provider.Channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumerTag: string.Empty,
                noLocal: false,
                exclusive: false,
                arguments: new Dictionary<string, object>(),
                consumer: consumer);

            _logger.LogDebug("Subscribed to channel {channel}", queueName);

            return Task.CompletedTask;
        }
    }
}
