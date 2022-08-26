namespace Aspose.Test.Html.Tests
{
    public class Tests
    {
        [Test]
        [TestCase("first <script>some script</script> second", "first  second")]
        [TestCase("first <script>some script</script>second", "first second")]
        [TestCase("first <script>some script", "first ")]
        [TestCase("first <script>some script</script> second <script attr='value'>script</script> third", "first  second  third")]
        [TestCase("first <script>some script</script> second <script attr='value'>", "first  second ")]
        public void RemoveScriptsTest(string src, string result)
        {
            var extractor = new TextExtractor(null, null);
            Assert.That(extractor.RemoveScripts(src), Is.EqualTo(result));
        }

        [Test]
        [TestCase("first second", "first second")]
        [TestCase("first <span>second</span>", "first second")]
        public void RemoveTagsTest(string src, string result)
        {
            var extractor = new TextExtractor(null, null);
            Assert.That(extractor.RemoveTags(src), Is.EqualTo(result));
        }

        [Test]
        [TestCase("<html><header></header><body>some <span>text</span><script>some script</script></body></html>", "some text")]
        [TestCase("<html><header><script>some script</script></header><body>some <span>text</span></body></html>", "some text")]
        public void GetTextTest(string src, string result)
        {
            var extractor = new TextExtractor(null, null);
            Assert.That(extractor.GetText(src), Is.EqualTo(result));
        }
    }
}