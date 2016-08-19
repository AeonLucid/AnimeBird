namespace AnitomyLib.Keywords
{
    internal class Keyword
    {

        public Keyword(ElementCategory category, KeywordOptions options)
        {
            Category = category;
            Options = options;
        }

        public ElementCategory Category { get; private set; }
        public KeywordOptions Options { get; private set; }

    }
}