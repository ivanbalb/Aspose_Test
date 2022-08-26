using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Counter.Tests
{
    public class WordSplitterTests
    {
        private static IEnumerable<TestCaseData> simpleTestCases()
        {
            yield return new TestCaseData("one two",
                            new WordResult[]
                            {
                                new WordResult("one", 1, 0),
                                new WordResult("two", 1, 0)
                            },
                            new WordResult[]
                            {
                                new WordResult("one two", 1, 0)
                            },
                            new WordResult[]
                            {
                            });
            yield return new TestCaseData("one two one",
                            new WordResult[]
                            {
                                new WordResult("one", 2, 0),
                                new WordResult("two", 1, 0)
                            },
                            new WordResult[]
                            {
                                new WordResult("one two", 1, 0),
                                new WordResult("two one", 1, 0)
                            },
                            new WordResult[]
                            {
                                new WordResult("one two one", 1, 0)
                            });
            yield return new TestCaseData("one one one",
                            new WordResult[]
                            {
                                new WordResult("one", 3, 0),
                            },
                            new WordResult[]
                            {
                                new WordResult("one one", 2, 0),
                            },
                            new WordResult[]
                            {
                                new WordResult("one one one", 1, 0)
                            });
        }

        [Test, TestCaseSource(nameof(simpleTestCases))]
        public void TestSplit(string src,
            IEnumerable<WordResult> one, 
            IEnumerable<WordResult> two, 
            IEnumerable<WordResult> three)
        {
            var splitter = new WordSplitter();
            splitter.Split(src);

            Assert.That(splitter.OneWord.OrderBy(r => r.word), Is.EqualTo(one));
            Assert.That(splitter.TwoWords.OrderBy(r => r.word), Is.EqualTo(two));
            Assert.That(splitter.ThreeWords.OrderBy(r => r.word), Is.EqualTo(three));
        }

        private static IEnumerable<TestCaseData> additiveTestCases()
        {
            yield return new TestCaseData(new[] { "one two", "two three" },
                            new WordResult[]
                            {
                                new WordResult("one", 1, 0),
                                new WordResult("three", 1, 0),
                                new WordResult("two", 2, 0)
                            },
                            new WordResult[]
                            {
                                new WordResult("one two", 1, 0),
                                new WordResult("two three", 1, 0)
                            },
                            new WordResult[]
                            {
                            });
            yield return new TestCaseData(new[] { "one two", "two three", "one two three" },
                            new WordResult[]
                            {
                                new WordResult("one", 2, 0),
                                new WordResult("three", 2, 0),
                                new WordResult("two", 3, 0)
                            },
                            new WordResult[]
                            {
                                new WordResult("one two", 2, 0),
                                new WordResult("two three", 2, 0)
                            },
                            new WordResult[]
                            {
                                new WordResult("one two three", 1, 0)
                            });
        }

        [Test, TestCaseSource(nameof(additiveTestCases))]
        public void TestSplitAdditive(string[] src,
            IEnumerable<WordResult> one,
            IEnumerable<WordResult> two,
            IEnumerable<WordResult> three)
        {
            var splitter = new WordSplitter();
            Array.ForEach(src, s => splitter.Split(s));

            Assert.That(splitter.OneWord.OrderBy(r => r.word), Is.EqualTo(one));
            Assert.That(splitter.TwoWords.OrderBy(r => r.word), Is.EqualTo(two));
            Assert.That(splitter.ThreeWords.OrderBy(r => r.word), Is.EqualTo(three));
        }
    }
}
