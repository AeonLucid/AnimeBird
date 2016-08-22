using AnitomyLib.Tokens;

namespace AnitomyLib.Extensions
{
    internal static class TokenExtensions
    {
        internal static bool IsDelimiter(this Token token)
        {
            return token.Category == TokenCategory.Delimiter;
        }

        internal static bool IsUnknown(this Token token)
        {
            return token.Category == TokenCategory.Unknown;
        }

        internal static bool IsSingleCharacter(this Token token)
        {
            return token.IsUnknown() && token.Content.Length == 1 && token.Content[0] != '-';
        }

        internal static void AppendTo(this Token token, Token appendTo)
        {
            appendTo.Content = appendTo.Content + token.Content;
            token.Category = TokenCategory.Invalid;
        }
    }
}
