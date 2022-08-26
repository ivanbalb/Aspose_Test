using Aspose.Test.Storage;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("Aspose.Test.Html.Tests")]

namespace Aspose.Test.Html
{
    public class TextExtractor : ITextExtractor
    {
        private readonly Regex regexHtmlScriptStart = new Regex("<script[^>]*>");
        private readonly Regex regexHtmlScriptEnd = new Regex("</script[^>]*>");

        private readonly Regex regexTag = new Regex("<[^>]+>");

        private readonly ILogger<TextExtractor> _logger;
        private readonly IStorage<Job> _storage;

        public TextExtractor(ILogger<TextExtractor> logger, IStorage<Job> storage)
        {
            _logger = logger;
            _storage = storage;
        }

        public async Task ExtractText(Guid jobId)
        {
            var job = await _storage.GetAsync(jobId);
            job = job.ChangeStatus(JobStatus.Parsing);
            await _storage.SaveAsync(job);

            job = job with { Content = GetText(job.Content) };
            job = job.ChangeStatus(JobStatus.Parsed);
            await _storage.SaveAsync(job);
        }

        public string GetText(string src)
        {
            var result = RemoveScripts(src);
            return RemoveTags(result);
        }

        internal string RemoveTags(string src)
        {
            var result = new StringBuilder();
            var currentPos = 0;
            var match = regexTag.Match(src);
            while(match.Success)
            {
                result.Append(src.Substring(currentPos, match.Index - currentPos));
                currentPos = match.Index + match.Length;

                match = regexTag.Match(src, currentPos);
            }
            if (currentPos == 0) return src;

            result.Append(src.Substring(currentPos));
            return result.ToString();
        }

        internal string RemoveScripts(string src)
        {
            var result = new StringBuilder();
            var currentPos = 0;
            var startMatch = regexHtmlScriptStart.Match(src, currentPos);
            while (startMatch.Success)
            {
                result.Append(src.Substring(currentPos, startMatch.Index - currentPos));
                var endMatch = regexHtmlScriptEnd.Match(src, startMatch.Index);
                currentPos = endMatch.Success ? endMatch.Index + endMatch.Length : src.Length;
                startMatch = regexHtmlScriptStart.Match(src, currentPos);
            }
            result.Append(src.Substring(currentPos));

            return result.ToString();
        }
    }
}