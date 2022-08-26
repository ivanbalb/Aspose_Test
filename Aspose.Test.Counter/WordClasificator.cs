using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Counter
{
    public class WordClasificator : IWordClasificator
    {
        private readonly HashSet<string> gramarWords = new HashSet<string>()
        {
            "you",
            "a",
            "the",
            "to",
            "is",
            "are"
        };

        public bool IsGramar(string word)
        {
            return gramarWords.Contains(word);
        }
    }
}
