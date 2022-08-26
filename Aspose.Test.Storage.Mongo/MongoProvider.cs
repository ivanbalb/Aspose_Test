using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Aspose.Test.Storage.Mongo
{
    public class MongoProvider : IMongoProvider
    {
        public IMongoDatabase Database { get; init; }

        public MongoProvider(ILogger<JobStorage> logger, IConfiguration config)
        {
            var settings = config.GetRequiredSection("MongoStorage").Get<MongoStorageSettings>();

            var client = new MongoClient(settings.ConnectionString);
            Database = client.GetDatabase(settings.DatabaseName);
            logger.LogDebug("MongoDB namespace for job storage: {namespaxe}", Database.DatabaseNamespace);
        }
    }
}
