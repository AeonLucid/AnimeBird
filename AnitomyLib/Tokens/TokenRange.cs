namespace AnitomyLib.Tokens
{
    internal class TokenRange
    {

        public TokenRange()
        {
            Offset = 0;
            Size = 0;
        }

        public TokenRange(int offset, int size)
        {
            Offset = offset;
            Size = size;
        }

        public int Offset { get; set; }

        public int Size { get; set; }

        public override string ToString()
        {
            return $"TokenRange{{Offset({Offset}) Size({Size})}}";
        }
    }
}