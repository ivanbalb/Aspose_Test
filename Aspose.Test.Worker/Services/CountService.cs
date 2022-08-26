using Aspose.Test.Counter;
using Aspose.Test.Messaging;
using Microsoft.Extensions.Logging;

namespace Aspose.Test.Worker.Services
{
    internal class CountService : ConsumerService<JobParsedMessage>
    {
        private readonly ILogger<CountService> _logger;
        private readonly ICounter _counter;

        public CountService(ILogger<CountService> logger,
            IConsumer<JobParsedMessage> consumer,
            ICounter counter) : base(logger, consumer)
        {
            _logger = logger;
            _counter = counter;
        }

        protected override async Task HandleMessage(JobParsedMessage message)
        {
            await _counter.CountJob(message.Id);
        }
    }
}
