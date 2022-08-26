using System.Linq;
using Moq;

namespace Aspose.Test.Counter.Tests
{
    public class CounterTests
    {
        private const string srcText = "Split PDF document online is a web service that allows you to split your " +
            "PDF document into separate pages. This simple application has several modes of operation, you can split your " +
            "PDF document into separate pages, i.e. each page of the original document will be a separate PDF document, " +
            "you can split your document into even and odd pages, this function will come in handy if you need to print a " +
            "document in the form of a book, you can also specify page numbers in the settings and the Split PDF application " +
            "will create separate PDF documents only with these pages and the fourth mode of operation allows you to create a " +
            "new PDF document in which there will be only those pages that you specified.";

        private readonly WordResult[] oneWordResult = new[]
        {
            new WordResult("document", 8, 6.3f),
            new WordResult("pdf", 7, 5.5f),
            new WordResult("split", 5, 3.9f),
            new WordResult("pages", 5, 3.9f),
            new WordResult("separate", 4, 3.1f)
        };

        private readonly WordResult[] oneWordResultWithGramar = new[]
        {
            new WordResult("document", 8, 6.3f),
            new WordResult("pdf", 7, 5.5f),
            new WordResult("you", 7, 5.5f),
            new WordResult("split", 5, 3.9f),
            new WordResult("a", 5, 3.9f)
        };

        private readonly WordResult[] twoWordsResult = new[]
        {
            new WordResult("pdf document", 5, 4.2f),
            new WordResult("split your", 3, 2.5f),
            new WordResult("document into", 3, 2.5f),
            new WordResult("you can", 3, 2.5f),
            new WordResult("split pdf", 2, 1.7f)
        };

        private readonly WordResult[] threeWordsResult = new[]
        {
            new WordResult("allows you to", 2, 1.8f),
            new WordResult("split your pdf", 2, 1.8f),
            new WordResult("your pdf document", 2, 1.8f),
            new WordResult("pdf document into", 2, 1.8f),
            new WordResult("document into separate", 2, 1.8f)
        };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task CountIgnoringGranar()
        {
            var counter = new Counter(null, null, new WordClasificator());

            var result = await counter.CountAsync(srcText.ToLower());

            Array.ForEach(oneWordResult, r => Assert.Contains(r, result.oneWord));
            Array.ForEach(twoWordsResult, r => Assert.Contains(r, result.twoWords));
            Array.ForEach(threeWordsResult, r => Assert.Contains(r, result.threeWords));
        }

        [Test]
        public async Task CountWithGranar()
        {
            var counter = new Counter(null, null, null);

            var result = await counter.CountAsync(srcText.ToLower(), false);

            Array.ForEach(oneWordResult, r => Assert.Contains(r, result.oneWord));
            Array.ForEach(twoWordsResult, r => Assert.Contains(r, result.twoWords));
            Array.ForEach(threeWordsResult, r => Assert.Contains(r, result.threeWords));
        }
    }
}