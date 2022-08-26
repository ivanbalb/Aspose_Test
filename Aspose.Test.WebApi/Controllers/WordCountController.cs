using Aspose.Test.Messaging;
using Aspose.Test.Storage;
using Aspose.Test.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aspose.Test.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordCountController : ControllerBase
    {
        private readonly ILogger<WordCountController> _logger;
        private readonly IStorage<Job> _storage;
        private readonly IPublisher<JobMessage> _publisher;

        public WordCountController(ILogger<WordCountController> logger,
            IStorage<Job> storage,
            IPublisher<JobMessage> publisher)
        {
            _logger = logger;
            _storage = storage;
            _publisher = publisher;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> StartCount([FromBody]CountRequest request)
        {
            try
            {
                var job = new Job(
                    Id: Guid.NewGuid(),
                    Status: JobStatus.Created,
                    Target: new Uri(request.url),
                    Messages: new List<JobHistoryMessage>(),
                    Content: default,
                    Result: default);
                job = job.AddInfo("Job created");

                await _storage.SaveAsync(job);
                await _publisher.PublishAsync(new JobCreatedMessage(job.Id));

                return Ok(new CountResponse(true, job.Id, String.Empty));
            }
            catch (Exception ex)
            {
                return BadRequest(new CountResponse(false, Guid.Empty, ex.Message));
            }
        }

        [HttpGet]
        [Route("{jobId}/{lastMessageTimestamp?}")]
        public async Task<IActionResult> GetJobStatus(Guid jobId, long? lastMessageTimestamp)
        {
            var job = await _storage.GetAsync(jobId);
            if (job == null) return NotFound();

            return Ok(new StatusResponse(job.Id, job.Status,
                lastMessageTimestamp.HasValue ? job.Messages.Where(m => m.Timestamp > lastMessageTimestamp).ToList()
                : job.Messages));
        }

        [HttpGet]
        [Route("{jobId}/result/{resultCount}")]
        public async Task<IActionResult> GetJobResult(Guid jobId, int resultCount)
        {
            var job = await _storage.GetAsync(jobId);
            if (job == null) return NotFound();
            if (job.Status < JobStatus.Done) return BadRequest("Job not finished");

            return Ok(new ResultsResponse(
                job.Result.OneWord.Take(resultCount).ToList(),
                job.Result.TwoWords.Take(resultCount).ToList(),
                job.Result.ThreeWords.Take(resultCount).ToList()
                ));
        }
    }
}
