using SCHackathon.Foundation.SimilarImagesSearch;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System.Linq;

namespace SCHackathon.Feature.SimilarImageSearch.ComputedFields
{
    public class ColorDominanceTable : AbstractComputedIndexField
    {
        private const string contentDB = "master";
        private const string settingsPath = "/sitecore/System/Settings/Feature/Similar Image Search/Indexing Settings";
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item == null) { return null; }

            var mediaItem = (MediaItem)item;

            var media = MediaManager.GetMedia(mediaItem);
            bool isPicture = mediaItem.MimeType.StartsWith("image/");

            if (isPicture == false)
                return null;

            var settingsItem = Sitecore.Data.Database.GetDatabase(contentDB).GetItem(settingsPath);
            if (settingsItem == null)
                return null;

            ID indexingScope = null;//= settingsItem["Indexing Scope"];

            if (ID.TryParse(settingsItem["Indexing Scope"], out indexingScope) == false)
                indexingScope = ID.Parse("{3D6658D8-A0BF-4E75-B3E2-D050FABCF4E1}");

            var scopeItem = Sitecore.Data.Database.GetDatabase(contentDB).GetItem(indexingScope);

            if (item.Paths.FullPath.StartsWith(scopeItem.Paths.FullPath) == false)
                return null;

            var stream = media.GetStream();

            if (stream == null)
                return null;

            //Extract and get dominance table for current image
            var domTable = stream.Stream.ExtractQuadrantsColorDominance();

            //Concatenate dominance table into a string and save into index
            return string.Join("|", domTable.Select(s => string.Join(",", s.Select(f => f.PercentageDiff))));
        }
    }
}