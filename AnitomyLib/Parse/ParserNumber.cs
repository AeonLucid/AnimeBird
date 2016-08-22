using System;
using System.Collections.Generic;
using AnitomyLib.Keywords;
using AnitomyLib.Tokens;

namespace AnitomyLib.Parse
{
    internal static class ParserNumber
    {
        private static readonly char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static bool SearchForEpisodePatterns(List<Token> tokens)
        {
            Console.WriteLine("ZOEK DAN " + tokens.Count);

            foreach (var token in tokens)
            {
                Console.WriteLine("ZOEK DAN " + token.Content);
                if (!char.IsDigit(token.Content[0]))
                {
                    if (NumberComesAfterPrefix(ElementCategory.EpisodePrefix, token))
                        return true;
                    if (NumberComesAfterPrefix(ElementCategory.VolumePrefix, token))
                        continue;
                }
                else
                {
                    
                }
            }

            return false;
        }

        private static bool NumberComesAfterPrefix(ElementCategory category, Token token)
        {
            var numberBegin = token.Content.IndexOfAny(Numbers);
            var prefix = KeywordManager.Normalize(token.Content.Substring(0, numberBegin));

            Console.WriteLine($"PREEEEEEEFIX: {prefix}");

            return false;
        }
    }
}
