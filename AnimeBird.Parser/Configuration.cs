using System.Collections.Generic;

namespace AnimeBird.Parser
{
    internal static class Configuration
    {

        public static readonly List<char> Delimiters = new List<char>()
        {
            ' ',
            '_',
            '.',
            '&',
            '+',
            ',',
            '|'
        };

    }
}
