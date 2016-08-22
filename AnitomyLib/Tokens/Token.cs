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
        
        public TokenCategory Category { get; set; }
        public string Content { get; set; }
        public bool Enclosed { get; }

        public override string ToString()
        {
            return $"Token{{Category({Category}) Content({Content}) Enclosed({Enclosed})}}";
        }
    }
}
