using System;
using System.Collections.Generic;
using System.Linq;
using AnitomyLib.Extensions;
using AnitomyLib.Tokens;

namespace AnitomyLib.Parse
{
    internal static class ParserHelper
    {

        private static readonly Dictionary<string, string> Ordinals = new Dictionary<string, string>
        {
            { "1st", "1" }, { "First", "1" },
            { "2st", "2" }, { "Second", "2" },
            { "3st", "3" }, { "Third", "3" },
            { "4st", "4" }, { "Fourth", "4" },
            { "5st", "5" }, { "Fifth", "5" },
            { "6st", "6" }, { "Sixth", "6" },
            { "7st", "7" }, { "Seventh", "7" },
            { "8st", "8" }, { "Eighth", "8" },
            { "9st", "9" }, { "Ninth", "9" }
        };

        public static bool CheckAnimeSeasonKeyword(Dictionary<ElementCategory, string> elements, List<Token> tokens, Tuple<int, Token> tokenData)
        {
            var previousToken = tokens.FindPreviousToken(tokenData.Item1, TokenFlag.NotDelimiter);
            if (previousToken != null)
            {
                var number = GetNumberFromOrdinal(previousToken.Item2.Content);
                if (!string.IsNullOrEmpty(number))
                {
                    elements.Add(ElementCategory.AnimeSeason, number);
                    previousToken.Item2.Category = TokenCategory.Identifier;
                    tokenData.Item2.Category = TokenCategory.Identifier;
                    return true;
                }
            }

            var nextToken = tokens.FindNextToken(tokenData.Item1, TokenFlag.NotDelimiter);
            if (nextToken != null && nextToken.Item2.Content.All(char.IsDigit))
            {
                elements.Add(ElementCategory.AnimeSeason, nextToken.Item2.Content);
                nextToken.Item2.Category = TokenCategory.Identifier;
                tokenData.Item2.Category = TokenCategory.Identifier;
                return true;
            }

            return false;
        }

        public static void CheckExtentKeyword(Dictionary<ElementCategory, string> elements, List<Token> tokens, Tuple<int, Token> tokenData, ElementCategory category)
        {
            var nextToken = tokens.FindNextToken(tokenData.Item1, TokenFlag.NotDelimiter);
            if (nextToken.Item2.IsUnknown())
            {
                // TODO: Implement
                throw new NotImplementedException("CheckExtentKeyword");
            }
        }

        public static bool IsCrc32(string word)
        {
            return word.Length == 8 && IsHex(word);
        }

        public static bool IsResolution(string word)
        {
            Console.WriteLine("Resolution check: " + word);

            // *###x###*
            if (word.Length >= 7)
            {
                var pos = word.IndexOf('x');

                if (pos == -1)
                    pos = word.IndexOf('X');

                if (pos == -1)
                    pos = word.IndexOf('\u00D7');

                if (pos != -1)
                {
                    for (var i = 0; i < word.Length; i++)
                    {
                        if (i != pos && !char.IsDigit(word[i]))
                            return false;
                    }
                    return true;
                }
            }
            // *###p
            else if (word.Length >= 4)
            {
                if (word.EndsWith("p") || word.EndsWith("P"))
                {
                    return word.Take(word.Length - 1).All(char.IsDigit);
                }
            }

            return false;
        }

        private static bool IsHex(IEnumerable<char> chars)
        {
            foreach (var c in chars)
            {
                var isHex = (c >= '0' && c <= '9') ||
                            (c >= 'a' && c <= 'f') ||
                            (c >= 'A' && c <= 'F');

                if (!isHex)
                    return false;
            }
            return true;
        }

        private static string GetNumberFromOrdinal(string word)
        {
            return Ordinals.ContainsKey(word) ? Ordinals[word] : string.Empty;
        }
    }
}
