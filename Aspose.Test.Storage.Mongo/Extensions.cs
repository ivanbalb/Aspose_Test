using Microsoft.Extensions.DependencyInjection;

namespace Aspose.Test.Storage.Mongo
{
    public static class Extensions
    {
        public static IServiceCollection AddMongoStorage(this IServiceCollection services)
        {
            services.AddSingleton<IMongoProvider, MongoProvider>();
            services.AddScoped<IStorage<Job>, JobStorage>();

            return services;
        }
    }
}
