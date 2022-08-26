namespace Aspose.Test.Counter
{
    public record WordResult(string word, int count, float percentage);

    public record CountResult(WordResult[] oneWord, WordResult[] twoWords, WordResult[] threeWords);
}
