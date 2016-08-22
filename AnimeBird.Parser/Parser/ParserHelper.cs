using AnimeBird.Parser.Elements;
using AnimeBird.Parser.Keywords;

namespace AnimeBird.Parser.Parser
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

    }
}
