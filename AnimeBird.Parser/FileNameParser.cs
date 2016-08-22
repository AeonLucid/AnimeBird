using System.Collections.Generic;
using AnimeBird.Parser.Elements;
using AnimeBird.Parser.Parser;
using AnimeBird.Parser.Tokens;

namespace AnimeBird.Parser
{
    /// <summary>
    /// Extracts useful information from filenames of anime uploads.
    /// </summary>
    public class FileNameParser
    {
        private readonly string _fileName;
        
        public FileNameParser(string fileName)
        {
            _fileName = fileName;
            Tokens = new List<Token>();
            Elements = new Dictionary<ElementCategory, string>();
        }

        internal string FileName { get; private set; }

        internal List<Token> Tokens { get; }

        internal Dictionary<ElementCategory, string> Elements { get; }

        public bool Parse()
        {
            // Clean.
            Tokens.Clear();
            Elements.Clear();
            FileName = CheckExtension(_fileName);
            
            // Parse.
            var tokenizer = new Tokenizer(this);
            if (!tokenizer.Tokenize())
                return false;

            return true;
        }

        private string CheckExtension(string fileName)
        {
            string extension;
            if (ParserHelper.RemoveExtensionFromFileName(fileName, out fileName, out extension))
            {
                Elements.Add(ElementCategory.FileExtension, extension);
            }

            return fileName;
        }
    }
}
