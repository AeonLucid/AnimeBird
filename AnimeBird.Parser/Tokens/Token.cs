namespace AnimeBird.Parser.Tokens
{
    internal class Token
    {

        public Token(TokenCategory category, TokenRange range)
        {
            Category = category;
            Range = range;
        }

        public TokenCategory Category { get; }
        public TokenRange Range { get; }

    }
}
