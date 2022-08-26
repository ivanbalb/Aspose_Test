using MongoDB.Driver;

namespace Aspose.Test.Storage.Mongo
{
    public interface IMongoProvider
    {
        IMongoDatabase Database { get; }
    }
}
