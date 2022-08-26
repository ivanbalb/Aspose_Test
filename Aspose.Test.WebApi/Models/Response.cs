namespace Aspose.Test.WebApi.Models
{
    public record CountResponse(bool created, Guid jobId, string message);
    public record StatusResponse(Guid id, JobStatus status, ICollection<JobHistoryMessage> messages);
    public record ResultsResponse(ICollection<Stats> oneWord, ICollection<Stats> twoWords, ICollection<Stats> threeWords);
}
