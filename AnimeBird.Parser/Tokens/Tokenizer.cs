using System;
using System.Collections.Generic;
using AnimeBird.Parser.Extensions;
using AnimeBird.Parser.Keywords;

namespace AnimeBird.Parser.Tokens
{
    internal class Tokenizer
    {
        private readonly FileNameParser _parser;

        public Tokenizer(FileNameParser parser)
        {
            _parser = parser;
        }

        private static readonly List<Tuple<char, char>> BracketPairs = new List<Tuple<char, char>>
        {
            Tuple.Create('(', ')'), // U+0028-U+0029 Parenthesis
            Tuple.Create('[', ']'), // U+005B-U+005D Square bracket
            Tuple.Create('{', '}'), // U+007B-U+007D Curly bracket
            Tuple.Create('\u300C', '\u300D'), // Corner bracket
            Tuple.Create('\u300E', '\u300F'), // White corner bracket
            Tuple.Create('\u3010', '\u3011'), // Black lenticular bracket
            Tuple.Create('\uFF08', '\uFF09') // Fullwidth parenthesis
        };

        public bool Tokenize()
        {
            TokenizeBrackets();

            return _parser.Tokens.Count != 0;
        }

        private void FoundToken(TokenCategory category, TokenRange range)
        {
            var token = new Token(category, range);
            _parser.Tokens.Add(token);

            Console.WriteLine($"{token.Range.StartIndex, -4}|{token.Category, -12}|'{token.Range.Content}'");
        }
        
        /// <summary>
        /// First we check all the brackets in the <see cref="FileNameParser._fileName"/>
        /// All content not between brackets is sent to <see cref="TokenizeByPreidentified"/>.
        /// </summary>
        private void TokenizeBrackets()
        {
            var isOpen = false;
            var previousPosition = 0;
            var currentPosition = 0;
            var matchingBracket = '\0';

            while (currentPosition < _parser.FileName.Length)
            {
                currentPosition = isOpen ? _parser.FileName.IndexOf(matchingBracket, currentPosition) : FindOpeningBracket(currentPosition, out matchingBracket);

                if (currentPosition == -1)
                    currentPosition = _parser.FileName.Length;

                var range = new TokenRange(_parser.FileName, previousPosition, currentPosition);
                if (range.Length > 0)
                    TokenizeByPreidentified(range);

                if (currentPosition != _parser.FileName.Length)
                {
                    FoundToken(TokenCategory.Bracket, new TokenRange(_parser.FileName, currentPosition, ++currentPosition));
                    isOpen = !isOpen;
                    previousPosition = currentPosition;
                }
            }
        }

        private void TokenizeByPreidentified(TokenRange range)
        {
            var preidentifiedTokens = KeywordManager.Peek(_parser, range);
            var currentIndex = range.StartIndex;
            var subrange = new TokenRange(_parser.FileName, range.StartIndex);

            while (currentIndex < range.EndIndex)
            {
                foreach (var preidentifiedToken in preidentifiedTokens)
                {
                    if (preidentifiedToken.StartIndex == currentIndex)
                    {
                        if (subrange.Length > 0)
                            TokenizeByDelimiters(subrange);

                        FoundToken(TokenCategory.Identifier, preidentifiedToken);
                        subrange.StartIndex = preidentifiedToken.EndIndex;
                        currentIndex = subrange.StartIndex - 1;
                        break;
                    }
                }

                currentIndex++;
                subrange.EndIndex = currentIndex;
            }

            if(subrange.Length > 0)
                TokenizeByDelimiters(subrange);
        }

        private void TokenizeByDelimiters(TokenRange range)
        {
            var delimiter = range.FindDelimiter();

            if (delimiter == '\0')
            {
                FoundToken(TokenCategory.Unknown, range);
                return;
            }

            var delimiterIndex = 0;
            var prevDelimiterIndex = delimiterIndex;

            while (delimiterIndex != range.Length)
            {
                delimiterIndex = range.Content.IndexOf(delimiter, prevDelimiterIndex);

                if (delimiterIndex == -1)
                    delimiterIndex = range.Length;

                var startIndex = prevDelimiterIndex + range.StartIndex;
                var subrange = new TokenRange(_parser.FileName, startIndex, startIndex + delimiterIndex - prevDelimiterIndex);
                if (subrange.Length > 0)
                    FoundToken(TokenCategory.Unknown, subrange);

                if (delimiterIndex != range.Length)
                {
                    FoundToken(TokenCategory.Delimiter, new TokenRange(_parser.FileName, subrange.EndIndex, subrange.EndIndex + 1));

                    prevDelimiterIndex = ++delimiterIndex;
                }
            }
        }

        private int FindOpeningBracket(int currentPosition, out char matchingBracket)
        {
            for (var i = currentPosition; i < _parser.FileName.Length; i++)
            {
                foreach (var bracketPair in BracketPairs)
                {
                    if (bracketPair.Item1 == _parser.FileName[i])
                    {
                        matchingBracket = bracketPair.Item2;
                        return i;
                    } 
                }
            }

            matchingBracket = char.MinValue;
            return _parser.FileName.Length;
        }
    }
}
