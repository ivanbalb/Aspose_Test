namespace Aspose.Test.WebApi.Models
{
    public record CountRequest(string url);
    public record CheckStatusRequest(Guid jobId, long? lastMessageTimestamp);
    public record GetResultsRequest(Guid jobId, int? resultsCount);
}
