using System;
using System.Collections.Generic;
using AnimeBird.Parser.Elements;
using AnimeBird.Parser.Tokens;

namespace AnimeBird.Parser.Keywords
{
    internal static class KeywordManager
    {
        private static readonly Dictionary<string, Keyword> FileExtensions;
        private static readonly Dictionary<string, Keyword> Keys;

        static KeywordManager()
        {
            FileExtensions = new Dictionary<string, Keyword>();
            Keys = new Dictionary<string, Keyword>();

            var optionsDefault = new KeywordOptions();
            var optionsInvalid = new KeywordOptions(true, true, true);
            var optionsUnidentifiable = new KeywordOptions(false, true, true);
            var optionsUnidentifiableInvalid = new KeywordOptions(true, true, false);
            var optionsUnidentifiableUnsearchable = new KeywordOptions(false, false, true);

            #region Keyword defaults
            Add(ElementCategory.AnimeSeasonPrefix, optionsUnidentifiable, new[]
            {
                "SAISON", "SEASON"
            });

            Add(ElementCategory.AnimeType, optionsUnidentifiable, new[]
            {
                "GEKIJOUBAN", "MOVIE",
                "OAD", "OAV", "ONA", "OVA",
                "SPECIA", "SPECIALS",
                "TV"
            });

            Add(ElementCategory.AnimeType, optionsUnidentifiableUnsearchable, new[]
            {
                "SP"
            }); // e.g. "Yumeiro Patissiere SP Professiona"

            Add(ElementCategory.AnimeType, optionsUnidentifiableInvalid, new[]
            {
                "ED", "ENDING", "NCED",
                "NCOP", "OP", "OPENING",
                "PREVIEW", "PV"
            });

            Add(ElementCategory.AudioTerm, optionsDefault, new[]
            {
                // Audio channels
                "2.0CH", "2CH", "5.1", "5.1CH", "DTS", "DTS-ES", "DTS5.1",
                "TRUEHD5.1",
                // Audio codec
                "AAC", "AACX2", "AACX3", "AACX4", "AC3", "FLAC", "FLACX2",
                "FLACX3", "FLACX4", "LOSSLESS", "MP3", "OGG", "VORBIS",
                // Audio language
                "DUALAUDIO", "DUAL AUDIO"
            });

            Add(ElementCategory.DeviceCompatibility, optionsDefault, new[]
            {
                "IPAD3", "IPHONE5", "IPOD", "PS3", "XBOX", "XBOX360"
            });

            Add(ElementCategory.DeviceCompatibility, optionsUnidentifiable, new[]
            {
                "ANDROID"
            });

            Add(ElementCategory.EpisodePrefix, optionsDefault, new[]
            {
                "EP", "EP.", "EPS", "EPS.", "EPISODE", "EPISODE.", "EPISODES",
                "CAPITULO", "EPISODIO", "FOLGE"
            });

            Add(ElementCategory.EpisodePrefix, optionsInvalid, new[]
            {
                "E", "\x7B2C"
            }); // single-letter episode keywords are not valid tokens

            Add(ElementCategory.FileExtension, optionsDefault, new[]
            {
                "3GP", "AVI", "DIVX", "FLV", "M2TS", "MKV", "MOV", "MP4", "MPG",
                "OGM", "RM", "RMVB", "WEBM", "WMV"
            });

            Add(ElementCategory.FileExtension, optionsInvalid, new[]
            {
                "AAC", "AIFF", "FLAC", "M4A", "MP3", "MKA", "OGG", "WAV", "WMA",
                "7Z", "RAR", "ZIP",
                "ASS", "SRT"
            });

            Add(ElementCategory.Language, optionsDefault, new[]
            {
                "ENG", "ENGLISH", "ESPANO", "JAP", "PT-BR", "SPANISH", "VOSTFR"
            });

            Add(ElementCategory.Language, optionsUnidentifiable, new[]
            {
                "ESP", "ITA"
            }); // e.g. "Tokyo ESP", "Bokura ga Ita"

            Add(ElementCategory.Other, optionsDefault, new[]
            {
                "REMASTER", "REMASTERED", "UNCENSORED", "UNCUT",
                "TS", "VFR", "WIDESCREEN", "WS"
            });

            Add(ElementCategory.ReleaseGroup, optionsDefault, new[]
            {
                "THORA"
            });

            Add(ElementCategory.ReleaseInformation, optionsDefault, new[]
            {
                "BATCH", "COMPLETE", "PATCH", "REMUX"
            });

            Add(ElementCategory.ReleaseInformation, optionsUnidentifiable, new[]
            {
                "END", "FINA"
            }); // e.g. "The End of Evangelion", "Final Approach"

            Add(ElementCategory.ReleaseVersion, optionsDefault, new[]
            {
                "V0", "V1", "V2", "V3", "V4"
            });

            Add(ElementCategory.Source, optionsDefault, new[]
            {
                "BD", "BDRIP", "BLURAY", "BLU-RAY",
                "DVD", "DVD5", "DVD9", "DVD-R2J", "DVDRIP", "DVD-RIP",
                "R2DVD", "R2J", "R2JDVD", "R2JDVDRIP",
                "HDTV", "HDTVRIP", "TVRIP", "TV-RIP",
                "WEBCAST", "WEBRIP"
            });

            Add(ElementCategory.Subtitles, optionsDefault, new[]
            {
                "ASS", "BIG5", "DUB", "DUBBED", "HARDSUB", "RAW", "SOFTSUB",
                "SOFTSUBS", "SUB", "SUBBED", "SUBTITLED"
            });

            Add(ElementCategory.VideoTerm, optionsDefault, new[]
            {
                // Frame rate
                "23.976FPS", "24FPS", "29.97FPS", "30FPS", "60FPS", "120FPS",
                // Video codec
                "8BIT", "8-BIT", "10BIT", "10BITS", "10-BIT", "10-BITS", "HI10P",
                "H264", "H265", "H.264", "H.265", "X264", "X265", "X.264",
                "AVC", "HEVC", "DIVX", "DIVX5", "DIVX6", "XVID",
                // Video format
                "AVI", "RMVB", "WMV", "WMV3", "WMV9",
                // Video quality
                "HQ", "LQ",
                // Video resolution
                "HD", "SD"
            });

            Add(ElementCategory.VolumePrefix, optionsDefault, new[]
            {
                "VO", "VOL.", "VOLUME"
            });
            #endregion
        }

        private static void Add(ElementCategory category, KeywordOptions options, IEnumerable<string> keywords)
        {
            var keywordContainer = GetKeywordContainer(category);

            foreach (var keyword in keywords)
            {
                if (string.IsNullOrEmpty(keyword))
                    continue;

                if (keywordContainer.ContainsKey(keyword))
                    throw new Exception($"Keyword '{keyword}' already exists.");

                keywordContainer.Add(keyword, new Keyword(category, options));
            }
        }

        public static bool Find(string keyword, ElementCategory category)
        {
            keyword = keyword.ToUpper();

            var keywordContainer = GetKeywordContainer(category);

            return keywordContainer.ContainsKey(keyword) && keywordContainer[keyword].Category == category;
        }

        public static bool Find(string keyword, ElementCategory category, KeywordOptions options, out ElementCategory outCategory, out KeywordOptions outOptions)
        {
            keyword = keyword.ToUpper();

            outCategory = category;
            outOptions = options;

            var keywordContainer = GetKeywordContainer(category);

            if (keywordContainer.ContainsKey(keyword))
            {
                var key = keywordContainer[keyword];
                if (category == ElementCategory.Unknown)
                {
                    outCategory = key.Category;
                }
                else if (key.Category != category)
                {
                    return false;
                }
                outOptions = key.Options;
                return true;
            }

            return false;
        }

        public static List<TokenRange> Peek(FileNameParser parser, TokenRange range)
        {
            var preidentifiedTokens = new List<TokenRange>();
            var entries = new List<Tuple<ElementCategory, List<string>>>
            {
                Tuple.Create(ElementCategory.AudioTerm, new List<string> { "DUAL AUDIO" }),
                Tuple.Create(ElementCategory.VideoTerm, new List<string> { "H264", "H.264" }),
                Tuple.Create(ElementCategory.VideoResolution, new List<string> { "480P", "720P", "1080P" }),
                Tuple.Create(ElementCategory.Source, new List<string> { "BLU-RAY" }),
            };

            var tokenPart = range.Content.ToUpper();

            foreach (var entry in entries)
            {
                foreach (var keyword in entry.Item2)
                {
                    var offsetPosition = tokenPart.IndexOf(keyword, StringComparison.Ordinal);
                    if (offsetPosition != -1)
                    { 
                        var startIndex = offsetPosition + range.StartIndex;
                        parser.Elements.Add(entry.Item1, keyword);
                        preidentifiedTokens.Add(new TokenRange(parser.FileName, startIndex, startIndex + keyword.Length));
                    }
                }
            }

            return preidentifiedTokens;
        }

        private static Dictionary<string, Keyword> GetKeywordContainer(ElementCategory category)
        {
            return category == ElementCategory.FileExtension ? FileExtensions : Keys;
        }
    }
}
