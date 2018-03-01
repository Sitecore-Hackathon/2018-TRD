using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.SitecoreExtensions.Extensions
{
    public static class MediaItemExtensions
    {
        public static string SizeAsReadableString([NotNull] this MediaItem mediaItem)
        {
            var pdfFileSize = mediaItem.Size;

            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (pdfFileSize >= 1024 && order < pdfFileSize - 1)
            {
                order++;
                pdfFileSize = pdfFileSize / 1024;
            }

            return String.Format("{0:0.##} {1}", pdfFileSize, sizes[order]);
        }
    }
}