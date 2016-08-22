namespace AnimeBird.Parser.Keywords
{
    internal class KeywordOptions
    {

        public KeywordOptions()
        {
            Identifiable = true;
            Searchable = true;
            Valid = true;
        }

        public KeywordOptions(bool identifiable, bool searchable, bool valid)
        {
            Identifiable = identifiable;
            Searchable = searchable;
            Valid = valid;
        }

        public bool Identifiable { get; private set; }
        public bool Searchable { get; private set; }
        public bool Valid { get; private set; }

    }
}
