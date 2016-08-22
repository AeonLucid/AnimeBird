using System;
using System.Collections.Generic;
using AnitomyLib.Tokens;

namespace AnitomyLib.Extensions
{
    internal static class TokenListExtensions
    {
        internal static Tuple<int, Token> FindPreviousToken(this List<Token> tokens, int currentIndex, TokenFlag flag)
        {
            return FindToken(tokens, currentIndex - 1, flag);
        }

        internal static Tuple<int, Token> FindNextToken(this List<Token> tokens, int currentIndex, TokenFlag flag)
        {
            return FindToken(tokens, currentIndex + 1, flag);
        }

        private static Tuple<int, Token> FindToken(List<Token> tokens, int index, TokenFlag flag)
        {
            if (index < 0 || index >= tokens.Count) return null;

            var token = Tuple.Create(index, tokens[index]);

            return CheckTokenFlag(token.Item2, flag) ? token : null;
        }

        private static bool CheckTokenFlag(Token token, TokenFlag flag)
        {
            var flagInt = (int) flag;
            if ((flagInt & (int) TokenFlag.Enclosed) > 0)
            {
                var success = CheckFlag(TokenFlag.Enclosed, flag) ? token.Enclosed : !token.Enclosed;
                if (!success)
                    return false;
            }

            if ((flagInt & (int) TokenFlag.MaskCategories) > 0)
            {
                bool success;

                success = CheckCategory(false, token, flag, TokenFlag.Bracket, TokenFlag.NotBracket, TokenCategory.Bracket);
                success = CheckCategory(success, token, flag, TokenFlag.Delimiter, TokenFlag.NotDelimiter, TokenCategory.Delimiter);
                success = CheckCategory(success, token, flag, TokenFlag.Identifier, TokenFlag.NotIdentifier, TokenCategory.Identifier);
                success = CheckCategory(success, token, flag, TokenFlag.Unknown, TokenFlag.NotUnknown, TokenCategory.Unknown);
                success = CheckCategory(success, token, flag, TokenFlag.NotValid, TokenFlag.Valid, TokenCategory.Invalid);

                if (!success)
                    return false;
            }

            return true;
        }

        private static bool CheckFlag(TokenFlag firstFlag, TokenFlag secondFlag)
        {
            return ((int) secondFlag & (int) firstFlag) == (int) firstFlag;
        }

        private static bool CheckCategory(bool success, Token token, TokenFlag flag, TokenFlag fe, TokenFlag fn, TokenCategory c)
        {
            if (success) return true;

            return CheckFlag(fe, flag)
                ? token.Category == c
                : CheckFlag(fn, flag) && (token.Category != c);
        }
    }
}
