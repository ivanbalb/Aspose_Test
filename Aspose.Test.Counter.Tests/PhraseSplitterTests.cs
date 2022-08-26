using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Counter.Tests
{
    public class PhraseSplitterTests
    {
        [Test]
        [TestCase("one phrase", new[] {"one phrase"})]
        [TestCase("two, phrases", new[] { "two", "phrases" })]
        [TestCase("three/3, phrases; test", new[] {"three 3", "phrases", "test"})]
        public void TestSplit(string src, string[] results)
        {
            var splitter = new PhraseSplitter();
            var result = splitter.Split(src).ToArray();

            Assert.That(result, Is.EqualTo(results));
        }
    }
}
