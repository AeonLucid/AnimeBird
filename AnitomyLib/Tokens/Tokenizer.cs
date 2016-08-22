using System;
using System.Collections.Generic;
using System.Linq;
using AnitomyLib.Extensions;
using AnitomyLib.Keywords;

namespace AnitomyLib.Tokens
{
    internal class Tokenizer
    {
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

        private readonly string _fileName;
        private readonly Dictionary<ElementCategory, string> _elements;
        private readonly List<Token> _tokens;

        private int _position;

        private bool _isBracketOpen = false;
        private char _matchingBracket = '\0';

        public Tokenizer(string fileName, Dictionary<ElementCategory, string> elements, List<Token> tokens)
        {
            _fileName = fileName;
            _elements = elements;
            _tokens = tokens;
        }

        public bool Tokenize()
        {
            TokenizeByBrackets();

            return _tokens.Count != 0;
        }

        private void TokenizeByBrackets()
        {
            var currentPosition = 0;

            while (currentPosition < _fileName.Length && _position < _fileName.Length)
            {
                currentPosition = !_isBracketOpen
                    ? FindOpeningBracket()
                    : _fileName.IndexOf(_matchingBracket, currentPosition);

                var range = new TokenRange(_position + 1, currentPosition - _position - 1);
                if (range.Size > 0)
                    TokenizeByPreidentified(_isBracketOpen, range);

                if (currentPosition != _fileName.Length)
                {
                    AddToken(TokenCategory.Bracket, true, new TokenRange(range.Offset + range.Size, 1));
                    _isBracketOpen = !_isBracketOpen;
                    _position = currentPosition++;
                }
            }
        }

        private void TokenizeByPreidentified(bool enclosed, TokenRange range)
        {
            var preidentifiedTokens = new List<TokenRange>();
            KeywordManager.Peek(_fileName, range, _elements, preidentifiedTokens);

            var offset = range.Offset;
            var subrange = new TokenRange(range.Offset, 0);

            while (offset < range.Offset + range.Size)
            {
                foreach (var preidentifiedToken in preidentifiedTokens)
                {
                    if (offset == preidentifiedToken.Offset)
                    {
                        if (subrange.Size > 0)
                            TokenizeByDelimiters(enclosed, subrange);

                        AddToken(TokenCategory.Identifier, enclosed, preidentifiedToken);
                        subrange.Offset = preidentifiedToken.Offset + preidentifiedToken.Size;
                        offset = subrange.Offset - 1; // It's going to be incremented below
                        break;
                    }
                }

                offset = offset + 1;
                subrange.Size = offset - subrange.Offset;
            }

            if(subrange.Size > 0)
                TokenizeByDelimiters(enclosed, subrange);
        }

        private void TokenizeByDelimiters(bool enclosed, TokenRange range)
        {
            var delimiter = GetDelimiter(range);

            if (string.IsNullOrEmpty(delimiter))
            {
                AddToken(TokenCategory.Unknown, enclosed, range);
                return;
            }

            var delimiterIndex = 0;
            var previousDelimiterIndex = delimiterIndex;
            var part = _fileName.Substring(range.Offset, range.Size);

            while (delimiterIndex != part.Length)
            {
                delimiterIndex = part.IndexOf(delimiter, previousDelimiterIndex, StringComparison.Ordinal);

                if (delimiterIndex == -1)
                    delimiterIndex = part.Length;

                var subrange = new TokenRange(previousDelimiterIndex + range.Offset, delimiterIndex - previousDelimiterIndex);
                if (subrange.Size > 0)
                    AddToken(TokenCategory.Unknown, enclosed, subrange);

                if (delimiterIndex != part.Length)
                {
                    AddToken(TokenCategory.Delimiter, enclosed, new TokenRange(subrange.Offset + subrange.Size, 1));

                    delimiterIndex += 1;
                    previousDelimiterIndex = delimiterIndex;
                }
            }

            ValidateDelimiterTokens();
        }
        
        private void ValidateDelimiterTokens()
        {
            for (var i = 0; i < _tokens.Count; i++)
            {
                var token = _tokens[i];

                if(token.Category != TokenCategory.Delimiter)
                    continue;

                var delimiter = token.Content[0];
                var prevToken = _tokens.FindPreviousToken(i, TokenFlag.Valid);
                var nextToken = _tokens.FindNextToken(i, TokenFlag.Valid);

                if (delimiter != ' ' && delimiter != '_')
                {
                    if (prevToken.Item2.IsSingleCharacter())
                    {
                        token.AppendTo(prevToken.Item2);
                        while (nextToken.Item2.IsUnknown())
                        {
                            nextToken.Item2.AppendTo(prevToken.Item2);
                            nextToken = _tokens.FindNextToken(nextToken.Item1, TokenFlag.Valid);
                            if (nextToken.Item2.IsDelimiter() && nextToken.Item2.Content[0] == delimiter)
                            {
                                nextToken.Item2.AppendTo(prevToken.Item2);
                                nextToken = _tokens.FindNextToken(nextToken.Item1, TokenFlag.Valid);
                            }
                        }
                        continue;
                    }

                    if (nextToken.Item2.IsSingleCharacter())
                    {
                        token.AppendTo(prevToken.Item2);
                        nextToken.Item2.AppendTo(prevToken.Item2);
                        continue;
                    }
                }

                if (prevToken.Item2.IsUnknown() && nextToken.Item2.IsDelimiter())
                {
                    var nextDelimiter = nextToken.Item2.Content[0];
                    if (delimiter != nextDelimiter && delimiter != ',')
                    {
                        if (nextDelimiter == ' ' || nextDelimiter == '_')
                        {
                            token.AppendTo(prevToken.Item2);
                        }
                    }
                }
            }

            _tokens.RemoveAll(x => x.Category == TokenCategory.Invalid);
        }

        private int FindOpeningBracket()
        {
            for (var i = _position; i < _fileName.Length; i++)
            {
                foreach (var bracketPair in BracketPairs)
                {
                    if (!_fileName[i].Equals(bracketPair.Item1)) continue;

                    _matchingBracket = bracketPair.Item2;
                    return i;
                }
            }

            return _fileName.Length;
        }

        // TODO: Check if correct..
        private string GetDelimiter(TokenRange range)
        {
            var delimiter = string.Empty;
            var tokenPart = _fileName.Substring(range.Offset, range.Size);

            foreach (var c in tokenPart)
            {
                if (char.IsLetterOrDigit(c)) continue;
                if (Options.AllowedDelimiters.Contains(c))
                {
                    delimiter = c.ToString();
                }
            }

            return delimiter;
        }

        private void AddToken(TokenCategory category, bool enclosed, TokenRange range)
        {
            var token = new Token(category, _fileName.Substring(range.Offset, range.Size), enclosed);
            _tokens.Add(token);

            Console.WriteLine($"Found Token: \"{token.Content}\" ({category})");
        }
    }
}
