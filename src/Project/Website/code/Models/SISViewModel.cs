using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCH.Project.Web.Models
{
    public class SISViewModel
    {
        public SISViewModel(ID imageId)
        {
            ImageId = imageId;
            var item = Sitecore.Context.Database.GetItem(imageId);
            if (item != null)
            {
                ImageTitle = item["Title"];
                AltText = item["Alt"];
                Description = item["Description"];
                Size = item["Size"];

                var theURL = Sitecore.Resources.Media.MediaManager.GetMediaUrl((Sitecore.Data.Items.MediaItem)item);
                URL = Sitecore.Resources.Media.HashingUtils.ProtectAssetUrl(theURL);
            }
        }

        public ID ImageId { get; private set; }
        public string ImageTitle { get; private set; }
        public string URL { get; private set; }
        public string AltText { get; private set; }
        public string Description { get; private set; }
        public string Size { get; private set; }
    }
}