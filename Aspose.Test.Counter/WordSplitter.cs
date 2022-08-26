using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Counter
{
    internal interface IWordSplitter
    {
        Task Split(string text);
    }

    internal class WordSplitter : IWordSplitter
    {
        private readonly ConcurrentDictionary<string, WordResult> oneWord = new ConcurrentDictionary<string, WordResult>();
        private readonly ConcurrentDictionary<string, WordResult> twoWords = new ConcurrentDictionary<string, WordResult>();
        private readonly ConcurrentDictionary<string, WordResult> threeWords = new ConcurrentDictionary<string, WordResult>();

        public IEnumerable<WordResult> OneWord => oneWord.Values;
        public IEnumerable<WordResult> TwoWords => twoWords.Values;
        public IEnumerable<WordResult> ThreeWords => threeWords.Values;

        public Task Split(string text)
        {
            var currentPos = 0;
            while(currentPos < text.Length)
            {
                var next = text.IndexOfAny(Counter.spaces, currentPos);
                var word = next > 0 ? text.Substring(currentPos, next - currentPos) : text.Substring(currentPos);
                var newCurrentPos = next > 0 ? next + 1 : text.Length;

                oneWord.AddOrUpdate(word, (w) => new WordResult(w, 1, 0), (w, r) => r with { count = r.count + 1 });

                if (next > 0 && next < text.Length)
                {
                    next = text.IndexOfAny(Counter.spaces, next+1);
                    word = next > 0 ? text.Substring(currentPos, next - currentPos) : text.Substring(currentPos);

                    twoWords.AddOrUpdate(word, (w) => new WordResult(w, 1, 0), (w, r) => r with { count = r.count + 1 });

                    if (next > 0 && next < text.Length)
                    {
                        next = text.IndexOfAny(Counter.spaces, next+1);
                        word = next > 0 ? text.Substring(currentPos, next - currentPos) : text.Substring(currentPos);

                        threeWords.AddOrUpdate(word, (w) => new WordResult(w, 1, 0), (w, r) => r with { count = r.count + 1 });
                    }
                }
                currentPos = newCurrentPos;
            }
            return Task.CompletedTask;
        }
    }
}
