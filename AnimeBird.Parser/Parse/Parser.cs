using System;
using System.Collections.Generic;
using System.Linq;
using AnimeBird.Parser.Elements;
using AnimeBird.Parser.Keywords;
using AnimeBird.Parser.Tokens;

namespace AnimeBird.Parser.Parse
{
    internal class Parser
    {
        private readonly FileNameParser _fileNameParser;

        private const int AnimeYearMin = 1900;
        private const int AnimeYearMax = 2050;
        private const int EpisodeNumberMax = AnimeYearMin - 1;
        private const int VolumeNumberMax = 20;

        public Parser(FileNameParser fileNameParser)
        {
            _fileNameParser = fileNameParser;
        }

        public void Parse()
        {
            SearchForKeywords();
            SearchForIsolatedNumbers();
            SearchForEpisodeNumbers();
            // AnimeTitle
            // ReleaseGroup
            // EpisodeTitle
        }

        private void SearchForKeywords()
        {
            foreach (var token in GetRemainingTokens())
            {
                var word = token.Range.Content.Replace(" -", "");

                if(token.Range.Length != 8 && token.Range.Content.All(char.IsDigit))
                    continue;

                var keywordOptions = new KeywordOptions();
                var category = ElementCategory.Unknown;

                if (KeywordManager.Find(word, category, keywordOptions, out category, out keywordOptions))
                {
                    if (!_fileNameParser.Elements.ContainsKey(ElementCategory.AnimeYear) && word.All(char.IsDigit))
                    {
                        
                    }
                }
                else
                {
                    if (!_fileNameParser.Elements.ContainsKey(ElementCategory.FileChecksum) && ParserHelper.IsCrc32(token.Range.Content))
                    {
                        category = ElementCategory.FileChecksum;
                    }
                    else if (!_fileNameParser.Elements.ContainsKey(ElementCategory.VideoResolution) && ParserHelper.IsResolution(token.Range.Content))
                    {
                        category = ElementCategory.VideoResolution;
                    }
                }

                if (category != ElementCategory.Unknown)
                {
                    _fileNameParser.Elements.Add(category, token.Range.Content);

                    if(keywordOptions.Identifiable)
                        token.Category = TokenCategory.Identifier;
                }
            }
        }

        private void SearchForIsolatedNumbers()
        {
            foreach (var token in GetRemainingTokens())
            {
                var tokenNode = _fileNameParser.Tokens.Find(token);

                if (!token.Range.Content.All(char.IsDigit) || !ParserHelper.IsTokenIsolated(tokenNode))
                    continue;

                var number = int.Parse(token.Range.Content);

                // Anime year
                if (number >= AnimeYearMin && number <= AnimeYearMax)
                {
                    if (!_fileNameParser.Elements.ContainsKey(ElementCategory.AnimeYear))
                    {
                        _fileNameParser.Elements.Add(ElementCategory.AnimeYear, token.Range.Content);
                        token.Category = TokenCategory.Identifier;
                        continue;
                    }
                }

                // Video resolution
                if (number == 480 || number == 720 || number == 1080)
                {
                    // If these numbers are isolated, it's more likely for them to be the
                    // video resolution rather than the episode number. Some fansub groups
                    // use these without the "p" suffix.
                    if (!_fileNameParser.Elements.ContainsKey(ElementCategory.VideoResolution))
                    {
                        _fileNameParser.Elements.Add(ElementCategory.VideoResolution, token.Range.Content);
                        token.Category = TokenCategory.Identifier;
                    }
                }
            }
        }

        private void SearchForEpisodeNumbers()
        {
            foreach (var token in GetRemainingTokens())
            {
                if(token.Range.Content.All(char.IsDigit))
                {
                    var number = int.Parse(token.Range.Content);
                    if (!_fileNameParser.Elements.ContainsKey(ElementCategory.EpisodeNumber) && number <= EpisodeNumberMax)
                    {
                        _fileNameParser.Elements.Add(ElementCategory.EpisodeNumber, token.Range.Content);
                        token.Category = TokenCategory.Identifier;
                        continue;
                    }
                }
            }
        }

        private IEnumerable<Token> GetRemainingTokens()
        {
            return _fileNameParser.Tokens.Where(x => x.Category == TokenCategory.Unknown);
        }
    }
}
