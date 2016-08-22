using System.Linq;
using AnimeBird.Parser.Tokens;

namespace AnimeBird.Parser.Extensions
{
    internal static class TokenRangeExtensions
    {
        public static char FindDelimiter(this TokenRange range)
        {
            return
                range.Content.Where(c => !char.IsLetterOrDigit(c))
                    .FirstOrDefault(c => Configuration.Delimiters.Contains(c));
        }
    }
}
