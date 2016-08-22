using System.Collections.Generic;
using AnimeBird.Parser.Elements;
using AnimeBird.Parser.Parse;
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
            Tokens = new LinkedList<Token>();
            Elements = new Dictionary<ElementCategory, string>();
        }

        internal string FileName { get; private set; }

        public LinkedList<Token> Tokens { get; }

        public Dictionary<ElementCategory, string> Elements { get; }

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

            var parser = new Parse.Parser(this);
            parser.Parse();

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
