namespace AnitomyLib.Extensions
{
    internal static class ElementCategoryExtensions
    {

        public static bool IsSearchable(this ElementCategory category)
        {
            return category == ElementCategory.AnimeSeasonPrefix || category == ElementCategory.AnimeType ||
                   category == ElementCategory.AudioTerm || category == ElementCategory.DeviceCompatibility ||
                   category == ElementCategory.EpisodePrefix || category == ElementCategory.FileChecksum ||
                   category == ElementCategory.Language || category == ElementCategory.Other ||
                   category == ElementCategory.ReleaseGroup || category == ElementCategory.ReleaseInformation ||
                   category == ElementCategory.ReleaseVersion || category == ElementCategory.Source ||
                   category == ElementCategory.Subtitles || category == ElementCategory.VideoResolution ||
                   category == ElementCategory.VideoTerm || category == ElementCategory.VolumePrefix;
        }

        public static bool IsSingular(this ElementCategory category)
        {
            return category != ElementCategory.AnimeSeason && category != ElementCategory.AnimeType &&
                   category != ElementCategory.AudioTerm && category != ElementCategory.DeviceCompatibility &&
                   category != ElementCategory.EpisodeNumber && category != ElementCategory.Language &&
                   category != ElementCategory.Other && category != ElementCategory.ReleaseInformation &&
                   category != ElementCategory.Source && category != ElementCategory.VideoTerm;
        }

    }
}
