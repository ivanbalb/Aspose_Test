using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Aspose.Test.Storage.Mongo
{
    public class JobStorage : IStorage<Job>
    {
        private readonly ILogger<JobStorage> _logger;
        private readonly IMongoProvider _provider;
        private readonly string _collectionName;

        public JobStorage(ILogger<JobStorage> logger, IMongoProvider provider, IConfiguration config)
        {
            _logger = logger;
            _provider = provider;
            _collectionName = config.GetRequiredSection("MongoStorage").Get<MongoStorageSettings>().JobsCollectionName;
        }

        public async Task<Job> GetAsync(Guid id)
        {
            _logger.LogDebug("Loading job Id = {id}", id);

            var col = _provider.Database.GetCollection<Job>(_collectionName);

            var filter = Builders<Job>.Filter.Eq("Id", id);
            return await (await col.FindAsync<Job>(filter)).FirstOrDefaultAsync();
        }

        public async Task SaveAsync(Job job)
        {
            _logger.LogDebug("Saving job Id = {id} Status = {status}", job.Id, job.Status);

            var col = _provider.Database.GetCollection<Job>(_collectionName);

            var filter = Builders<Job>.Filter.Eq("Id", job.Id);
            var exists = await(await col.FindAsync<Job>(filter)).FirstOrDefaultAsync();
            if (exists != null)
                await col.ReplaceOneAsync(filter, job);
            else
                await col.InsertOneAsync(job);
        }
    }
}
