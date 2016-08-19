﻿using System.Collections.Generic;
using System.Linq;
using AnitomyLib.Keywords;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
namespace AnitomyLib
{
    public class Anitomy
    {
        private readonly Dictionary<ElementCategory, string> _elements;

        public Anitomy()
        {
            _elements = new Dictionary<ElementCategory, string>();
        }

        public bool Parse(string fileName)
        {
            if (Options.ParseFileExtension)
            {
                string fileExtension;
                if(RemoveExtensionFromFilename(fileName, out fileName, out fileExtension))
                    _elements.Add(ElementCategory.FileExtension, fileExtension);
            }

            // TODO: Ignored strings

            if (string.IsNullOrEmpty(fileName))
                return false;

            _elements.Add(ElementCategory.FileName, fileName);

            // TODO: Tokenizer

            // TODO: Parser

            return true;
        }

        private static bool RemoveExtensionFromFilename(string fileName, out string fileNameOut, out string fileExtensionOut)
        {
            fileNameOut = fileName;
            fileExtensionOut = null;

            var dotPosition = fileName.LastIndexOf('.');
            
            if (dotPosition == -1)
                return false;
            
            fileExtensionOut = fileName.Substring(dotPosition + 1);

            if (fileExtensionOut.Length > 4)
                return false;

            if (!fileExtensionOut.All(char.IsLetterOrDigit))
                return false;

            if (!KeywordManager.Find(ElementCategory.FileExtension, KeywordManager.Normalize(fileExtensionOut)))
                return false;

            fileNameOut = fileName.Substring(0, dotPosition);

            return true;
        }
    }
}
