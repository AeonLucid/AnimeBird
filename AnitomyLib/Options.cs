using System.Collections.Generic;

namespace AnitomyLib
{
    internal static class Options
    {
        public static readonly List<char> AllowedDelimiters = new List<char>()
        {
            ' ',
            '_',
            '.',
            '&',
            '+',
            ',',
            '|'
        };

        public const bool ParseEpisodeNumber = true;

        public const bool ParseEpisodeTitle = true;

        public const bool ParseFileExtension = true;

        public const bool ParseReleaseGroup = true;
    }
}
