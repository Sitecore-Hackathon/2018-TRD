using SCHackathon.Foundation.SimilarImagesSearch;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCH.Project.Web
{
    public class ComputedField_Q1_Red : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item == null) { return null; }

            var mediaItem = (MediaItem)item;

            var media = MediaManager.GetMedia(mediaItem);
            var stream = media.GetStream();

            if (stream == null)
                return null;

            var domTable = stream.Stream.ExtractQuadrantsColorDominance();

            return string.Join("|", domTable.Select(s => string.Join(",", s.Select(f => f.PercentageDiff))));
        }
    }
}