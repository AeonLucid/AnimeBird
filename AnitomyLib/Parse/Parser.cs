using System;
using System.Collections.Generic;
using System.Linq;
using AnitomyLib.Extensions;
using AnitomyLib.Keywords;
using AnitomyLib.Tokens;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace AnitomyLib.Parse
{
    internal class Parser
    {
        private readonly Dictionary<ElementCategory, string> _elements;
        private readonly List<Token> _tokens;

        public Parser(Dictionary<ElementCategory, string> elements, List<Token> tokens)
        {
            _elements = elements;
            _tokens = tokens;
        }

        public bool Parse()
        {
            SearchForKeywords();
//            SearchForIsolatedNumbers();

            foreach (var keyValuePair in _elements)
            {
                Console.WriteLine($"{keyValuePair.Key}\t: {keyValuePair.Value}");
            }

            return false;
        }

        private void SearchForKeywords()
        {
            for (var index = 0; index < _tokens.Count; index++)
            {
                var tokenData = Tuple.Create(index, _tokens[index]);
                if (tokenData.Item2.Category != TokenCategory.Unknown)
                    continue;

                var word = tokenData.Item2.Content.Replace(" -", "");

                if(string.IsNullOrEmpty(word))
                    continue;

                if(word.Length != 8 && word.All(char.IsDigit))
                    continue;

                var keyword = KeywordManager.Normalize(word);
                var keywordOptions = new KeywordOptions();
                var category = ElementCategory.Unknown;

                if (KeywordManager.Find(keyword, category, keywordOptions, out category, out keywordOptions))
                {
                    if (!Options.ParseReleaseGroup && category == ElementCategory.ReleaseGroup)
                        continue;
                    if (!category.IsSearchable() || !keywordOptions.Searchable)
                        continue;
                    if (category.IsSingular() && _elements.ContainsKey(category))
                        continue;
                    if (category == ElementCategory.AnimeSeasonPrefix)
                    {
                        ParserHelper.CheckAnimeSeasonKeyword(_elements, _tokens, tokenData);
                        continue;
                    }
                    if (category == ElementCategory.EpisodePrefix)
                    {
                        if (keywordOptions.Valid)
                            ParserHelper.CheckExtentKeyword(_elements, _tokens, tokenData, ElementCategory.EpisodeNumber);
                        continue;
                    }
                    if (category == ElementCategory.ReleaseVersion)
                    {
                        word = word.Substring(1); // number without "v"
                    }
                    else if (category == ElementCategory.VolumePrefix)
                    {
                        ParserHelper.CheckExtentKeyword(_elements, _tokens, tokenData, ElementCategory.VolumeNumber);
                        continue;
                    }
                }
                else
                {
                    if (!_elements.ContainsKey(ElementCategory.FileChecksum) && ParserHelper.IsCrc32(word))
                    {
                        category = ElementCategory.FileChecksum;
                    }
                    else if (!_elements.ContainsKey(ElementCategory.VideoResolution) && ParserHelper.IsResolution(word))
                    {
                        category = ElementCategory.VideoResolution;
                    }
                }

                if (category != ElementCategory.Unknown)
                {
                    _elements.Add(category, word);

                    if(keywordOptions.Identifiable)
                        tokenData.Item2.Category = TokenCategory.Identifier;
                }
            }
        }
    }
}
