using Aspose.Test.Http;
using Aspose.Test.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Aspose.Test.Worker.Services
{
    public class DownloadService : ConsumerService<JobCreatedMessage>
    {
        private readonly ILogger<DownloadService> _logger;
        private readonly IDownloader _downloader;
        private readonly IPublisher<JobMessage> _publisher;

        public DownloadService(ILogger<DownloadService> logger, 
            IConsumer<JobCreatedMessage> consumer, 
            IDownloader downloader,
            IPublisher<JobMessage> publisher) : base(logger, consumer)
        {
            _logger = logger;
            _downloader = downloader;
            _publisher = publisher;
        }

        protected override async Task HandleMessage(JobCreatedMessage message)
        {
            await _downloader.Load(message.Id);
            await _publisher.PublishAsync(new JobDownloadedMessage(message.Id));
        }
    }
}
