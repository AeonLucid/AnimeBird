namespace AnitomyLib.Tokens
{
    internal class Token
    {

        public Token()
        {
            Category = TokenCategory.Unknown;
            Content = string.Empty;
            Enclosed = false;
        }

        public Token(TokenCategory category, string content, bool enclosed)
        {
            Category = category;
            Content = content;
            Enclosed = enclosed;
        }

        public TokenCategory Category { get; }
        public string Content { get; }
        public bool Enclosed { get; }

    }
}
