using Aspose.Test.Storage;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Aspose.Test.Counter.Tests")]

namespace Aspose.Test.Counter
{
    public class Counter : ICounter
    {
        internal static readonly char[] phraseSeparators = new[] { ',', '.', ':', ';', '"', '\'', '?', '!' };
        internal static readonly char[] spaces = new[] { ' ', '\t', '\r', '\n', '\\', '/', '|' };
        internal static readonly char spaceReplacer = ' ';

        internal enum CharClass
        {
            Char,
            Space,
            PhraseSplitter
        };

        private readonly ILogger<Counter> _logger;
        private readonly IStorage<Job> _storage;
        private readonly IWordClasificator _wordClasificator;
        private readonly PhraseSplitter _phraseSplitter;

        public Counter(ILogger<Counter> logger,
            IStorage<Job> storage,
            IWordClasificator wordClasificator)
        {
            _logger = logger;
            _storage = storage;
            _wordClasificator = wordClasificator;
            _phraseSplitter = new PhraseSplitter();
        }

        public async Task CountJob(Guid jobId)
        {
            var job = await _storage.GetAsync(jobId);
            job = job.ChangeStatus(JobStatus.Counting);
            await _storage.SaveAsync(job);

            var result = await CountAsync(job.Content);
            job = job with { Result = new JobResult(
                result.oneWord.Select(r => new Stats(r.word, r.count, r.percentage)).ToList(), 
                result.twoWords.Select(r => new Stats(r.word, r.count, r.percentage)).ToList(), 
                result.threeWords.Select(r => new Stats(r.word, r.count, r.percentage)).ToList()) 
            };
            job = job.ChangeStatus(JobStatus.Done);
            await _storage.SaveAsync(job);
        }

        public async Task<CountResult> CountAsync(string text, bool ignoreGramars = true)
        {
            var wordSplitter = new WordSplitter();

            await Parallel.ForEachAsync(_phraseSplitter.Split(text), (p, tocken) =>
            {
                wordSplitter.Split(p);
                return ValueTask.CompletedTask;
            });

            var taskOneWord = CalculateResult(wordSplitter.OneWord, ignoreGramars);
            var taskTwoWords = CalculateResult(wordSplitter.TwoWords, false);
            var taskThreeWords = CalculateResult(wordSplitter.ThreeWords, false);

            await Task.WhenAll(taskOneWord, taskTwoWords, taskThreeWords);

            return new CountResult(
                taskOneWord.Result.ToArray(),
                taskTwoWords.Result.ToArray(),
                taskThreeWords.Result.ToArray());
        }

        private async Task<IEnumerable<WordResult>> CalculateResult(IEnumerable<WordResult> src, bool skipGramar)
        {
            var total = src.AsParallel().Sum(r => r.count);
            if (skipGramar)
                src = src.Where(r => !_wordClasificator.IsGramar(r.word));
            return src.AsParallel().Select(
                r => r with { percentage = (float)Math.Round((float)r.count / (float)total * 100f, 1)})
                .OrderByDescending(r => r.count).ThenBy(r => r.word);
        }

        internal static CharClass ClassifyChar(char c)
        {
            if (phraseSeparators.Contains(c)) return CharClass.PhraseSplitter;
            if (spaces.Contains(c)) return CharClass.Space;
            return CharClass.Char;
        }
    }
}
