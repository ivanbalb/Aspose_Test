using Aspose.Test.Storage;
using Microsoft.Extensions.Logging;

namespace Aspose.Test.Http
{
    public class Downloader : IDownloader
    {
        private readonly HttpClient _httpClient;
        private readonly IStorage<Job> _storage;
        private readonly ILogger<Downloader> _logger;

        public Downloader(ILogger<Downloader> logger,
            IStorage<Job> storage,
            HttpClient httpClient)
        {
            _logger = logger;
            _storage = storage;
            _httpClient = httpClient;
        }

        public async Task Load(Guid jobId)
        {
            var job = await _storage.GetAsync(jobId);
            job = job.ChangeStatus(JobStatus.Downloading);
            await _storage.SaveAsync(job);

            var content = await _httpClient.GetStringAsync(job.Target);
            job = job with { Content = content };
            job = job.ChangeStatus(JobStatus.Downloaded);
            await _storage.SaveAsync(job);
        }
    }
}
