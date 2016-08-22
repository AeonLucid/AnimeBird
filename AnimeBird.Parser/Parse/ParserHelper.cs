using System.Collections.Generic;
using System.Linq;
using AnimeBird.Parser.Elements;
using AnimeBird.Parser.Extensions;
using AnimeBird.Parser.Keywords;
using AnimeBird.Parser.Tokens;

namespace AnimeBird.Parser.Parse
{
    internal static class ParserHelper
    {

        public static bool RemoveExtensionFromFileName(string fileName, out string fileNameOut, out string fileExtensionOut)
        {
            fileNameOut = fileName;
            fileExtensionOut = string.Empty;

            var dotPosition = fileName.LastIndexOf('.');
            if (dotPosition == -1)
                return false;

            var extension = fileName.Substring(dotPosition + 1);
            if (extension.Length > 4)
                return false;

            if (!KeywordManager.Find(extension, ElementCategory.FileExtension))
                return false;

            fileNameOut = fileNameOut.Substring(0, dotPosition);
            fileExtensionOut = extension;

            return true;
        }

        public static bool IsCrc32(string word)
        {
            return word.Length == 8 && IsHex(word);
        }

        public static bool IsResolution(string word)
        {
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

        public static bool IsTokenIsolated(LinkedListNode<Token> tokenNode)
        {
            var prevToken = tokenNode.PreviousUntilNot(TokenCategory.Delimiter);
            var nextToken = tokenNode.NextUntilNot(TokenCategory.Delimiter);

            return prevToken != null && prevToken.Value.Category == TokenCategory.Bracket &&
                   nextToken != null && nextToken.Value.Category == TokenCategory.Bracket;
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

    }
}
