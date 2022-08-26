using System.Text;

namespace Aspose.Test.Counter
{
    internal interface IPhraceSplitter
    {
        IEnumerable<string> Split(string text);
    }

    internal class PhraseSplitter : IPhraceSplitter
    {
        public IEnumerable<string> Split(string text)
        {
            var builder = new StringBuilder();
            var spaceAdded = true;

            foreach(var c in text)
            {
                switch (Counter.ClassifyChar(c))
                {
                    case Counter.CharClass.Char:
                        {
                            builder.Append(char.ToLower(c));
                            spaceAdded = false;
                        }
                        break;
                    case Counter.CharClass.Space:
                        {
                            if (!spaceAdded)
                            {
                                builder.Append(Counter.spaceReplacer);
                                spaceAdded = true;
                            }
                        }
                        break;
                    case Counter.CharClass.PhraseSplitter:
                        {
                            if (builder.Length > 0)
                            {
                                spaceAdded = true;
                                yield return builder.ToString();
                                builder.Clear();
                            }
                        }
                        break;
                };
            }
            if (builder.Length > 0)
                yield return builder.ToString();
        }
    }
}