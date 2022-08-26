using Aspose.Test.Html;
using Aspose.Test.Messaging;
using Microsoft.Extensions.Logging;

namespace Aspose.Test.Worker.Services
{
    public class ParseService : ConsumerService<JobDownloadedMessage>
    {
        private readonly ILogger<ParseService> _logger;
        private readonly ITextExtractor _extractor;
        private readonly IPublisher<JobMessage> _publisher;

        public ParseService(ILogger<ParseService> logger,
            IConsumer<JobDownloadedMessage> consumer,
            ITextExtractor extractor,
            IPublisher<JobMessage> publisher) : base(logger, consumer)
        {
            _logger = logger;
            _publisher = publisher;
            _extractor = extractor;
        }

        protected override async Task HandleMessage(JobDownloadedMessage message)
        {
            await _extractor.ExtractText(message.Id);
            await _publisher.PublishAsync(new JobParsedMessage(message.Id));
        }
    }
}
