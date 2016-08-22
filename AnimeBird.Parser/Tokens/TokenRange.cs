namespace AnimeBird.Parser.Tokens
{
    public class TokenRange
    {
        private readonly string _fileName;

        public TokenRange(string fileName, int startIndex)
        {
            _fileName = fileName;
            StartIndex = startIndex;
        }

        public TokenRange(string fileName, int startIndex, int endIndex)
        {
            _fileName = fileName;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public int StartIndex { get; set; } // previousPosition
        public int EndIndex { get; set; } // currentPosition
        public int Length => EndIndex - StartIndex;
        public string Content => Length > 0 ? _fileName.Substring(StartIndex, EndIndex - StartIndex) : string.Empty;
    }
}
