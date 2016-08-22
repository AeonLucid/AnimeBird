namespace AnimeBird.Parser.Tokens
{
    public class Token
    {

        public Token(TokenCategory category, TokenRange range, bool enclosed)
        {
            Category = category;
            Range = range;
            Enclosed = enclosed;
        }

        public TokenCategory Category { get; set; }
        public TokenRange Range { get; }
        public bool Enclosed { get; }

        public override string ToString()
        {
            return $"{Range.StartIndex,-4}|{Category,-12}|'{Range.Content}'";
        }
    }
}
