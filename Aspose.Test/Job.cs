namespace Aspose.Test
{
    public enum JobStatus
    {
        Created,
        Downloading,
        Downloaded,
        Parsing,
        Parsed,
        Counting,
        Done,
        Error
    }

    public record Job(Guid Id,
        JobStatus Status,
        Uri Target,
        ICollection<JobHistoryMessage> Messages,
        string Content,
        JobResult Result,
        long LastUpdated = 0
        ) : IEntity;

    public enum JobHistoryMessageType
    {
        Info,
        Warning,
        Error
    }

    public record JobHistoryMessage(JobHistoryMessageType Type, long Timestamp, string Message);
    public record Stats(string Word, int Count, float Percentage);
    public record JobResult(IReadOnlyCollection<Stats> OneWord,
        IReadOnlyCollection<Stats> TwoWords,
        IReadOnlyCollection<Stats> ThreeWords);

    public static class JobExtensions
    {
        public static Job AddInfo(this Job job, string message)
        {
            job.Messages.Add(new JobHistoryMessage
            (
                Type: JobHistoryMessageType.Info,
                Timestamp: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Message: message
            ));
            return job with { LastUpdated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() };
        }

        public static Job AddWarnimg(this Job job, string message)
        {
            job.Messages.Add(new JobHistoryMessage
            (
                Type: JobHistoryMessageType.Warning,
                Timestamp: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Message: message
            ));
            return job with { LastUpdated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() };
        }

        public static Job AddError(this Job job, string message)
        {
            job.Messages.Add(new JobHistoryMessage
            (
                Type: JobHistoryMessageType.Info,
                Timestamp: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Message: message
            ));
            return job with { LastUpdated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() };
        }

        public static Job ChangeStatus(this Job job, JobStatus status)
        {
            job.Messages.Add(new JobHistoryMessage
            (
                Type: JobHistoryMessageType.Info,
                Timestamp: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Message: $"Status changed from {job.Status} to {status}"
            ));
            return job with { LastUpdated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), Status = status };
        }
    }
}