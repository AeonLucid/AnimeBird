using System;
using System.Collections.Generic;
using AnimeBird.Parser.Elements;
using AnimeBird.Parser.Tokens;

namespace AnimeBird.Parser.Keywords
{
    internal static class KeywordManager
    {
        private static readonly Dictionary<ElementCategory, List<string>> Keywords;

        static KeywordManager()
        {
            Keywords = new Dictionary<ElementCategory, List<string>>();

            #region Keyword defaults
            Add(ElementCategory.AnimeSeasonPrefix, new[]
            {
                "SAISON", "SEASON"
            });

            Add(ElementCategory.AnimeType, new[]
            {
                "GEKIJOUBAN", "MOVIE",
                "OAD", "OAV", "ONA", "OVA",
                "SPECIA", "SPECIALS",
                "TV"
            });

            Add(ElementCategory.AnimeType, new[]
            {
                "SP"
            }); // e.g. "Yumeiro Patissiere SP Professiona"

            Add(ElementCategory.AnimeType, new[]
            {
                "ED", "ENDING", "NCED",
                "NCOP", "OP", "OPENING",
                "PREVIEW", "PV"
            });

            Add(ElementCategory.AudioTerm, new[]
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

            Add(ElementCategory.DeviceCompatibility, new[]
            {
                "IPAD3", "IPHONE5", "IPOD", "PS3", "XBOX", "XBOX360"
            });

            Add(ElementCategory.DeviceCompatibility, new[]
            {
                "ANDROID"
            });

            Add(ElementCategory.EpisodePrefix, new[]
            {
                "EP", "EP.", "EPS", "EPS.", "EPISODE", "EPISODE.", "EPISODES",
                "CAPITULO", "EPISODIO", "FOLGE"
            });

            Add(ElementCategory.EpisodePrefix, new[]
            {
                "E", "\x7B2C"
            }); // single-letter episode keywords are not valid tokens

            Add(ElementCategory.FileExtension, new[]
            {
                "3GP", "AVI", "DIVX", "FLV", "M2TS", "MKV", "MOV", "MP4", "MPG",
                "OGM", "RM", "RMVB", "WEBM", "WMV"
            });

            Add(ElementCategory.FileExtension, new[]
            {
                "AAC", "AIFF", "FLAC", "M4A", "MP3", "MKA", "OGG", "WAV", "WMA",
                "7Z", "RAR", "ZIP",
                "ASS", "SRT"
            });

            Add(ElementCategory.Language, new[]
            {
                "ENG", "ENGLISH", "ESPANO", "JAP", "PT-BR", "SPANISH", "VOSTFR"
            });

            Add(ElementCategory.Language, new[]
            {
                "ESP", "ITA"
            }); // e.g. "Tokyo ESP", "Bokura ga Ita"

            Add(ElementCategory.Other, new[]
            {
                "REMASTER", "REMASTERED", "UNCENSORED", "UNCUT",
                "TS", "VFR", "WIDESCREEN", "WS"
            });

            Add(ElementCategory.ReleaseGroup, new[]
            {
                "THORA"
            });

            Add(ElementCategory.ReleaseInformation, new[]
            {
                "BATCH", "COMPLETE", "PATCH", "REMUX"
            });

            Add(ElementCategory.ReleaseInformation, new[]
            {
                "END", "FINA"
            }); // e.g. "The End of Evangelion", "Final Approach"

            Add(ElementCategory.ReleaseVersion, new[]
            {
                "V0", "V1", "V2", "V3", "V4"
            });

            Add(ElementCategory.Source, new[]
            {
                "BD", "BDRIP", "BLURAY", "BLU-RAY",
                "DVD", "DVD5", "DVD9", "DVD-R2J", "DVDRIP", "DVD-RIP",
                "R2DVD", "R2J", "R2JDVD", "R2JDVDRIP",
                "HDTV", "HDTVRIP", "TVRIP", "TV-RIP",
                "WEBCAST", "WEBRIP"
            });

            Add(ElementCategory.Subtitles, new[]
            {
                "ASS", "BIG5", "DUB", "DUBBED", "HARDSUB", "RAW", "SOFTSUB",
                "SOFTSUBS", "SUB", "SUBBED", "SUBTITLED"
            });

            Add(ElementCategory.VideoTerm, new[]
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

            Add(ElementCategory.VolumePrefix, new[]
            {
                "VO", "VOL.", "VOLUME"
            });
#endregion
        }

        private static void Add(ElementCategory category, IEnumerable<string> keywords)
        {
            if (!Keywords.ContainsKey(category))
            {
                Keywords.Add(category, new List<string>(keywords));
            }
            else
            {
                Keywords[category].AddRange(keywords);
            }
        }

        public static bool Find(string keyword, ElementCategory category)
        {
            return Keywords.ContainsKey(category) && Keywords[category].Contains(keyword.ToUpper());
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
    }
}
