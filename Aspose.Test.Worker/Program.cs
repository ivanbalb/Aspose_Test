using Aspose.Test.Counter;
using Aspose.Test.Html;
using Aspose.Test.Http;
using Aspose.Test.Messaging.RebbitMQ;
using Aspose.Test.Storage.Mongo;
using Aspose.Test.Worker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile(
        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
        true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    var builder = Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddMongoStorage();
            services.AddRebbitMQMessaging();

            services.AddTransient<HttpClient>();
            services.AddTransient<IDownloader, Downloader>();
            services.AddTransient<ITextExtractor, TextExtractor>();
            services.AddSingleton<IWordClasificator, WordClasificator>();
            services.AddTransient<ICounter, Counter>();

            services.AddHostedService<DownloadService>();
            services.AddHostedService<ParseService>();
            services.AddHostedService<CountService>();
        });

    builder.UseSerilog((context, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration));

    var host = builder.Build();
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
