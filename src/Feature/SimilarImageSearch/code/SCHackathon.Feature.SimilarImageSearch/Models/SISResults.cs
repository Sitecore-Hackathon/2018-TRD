using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHackathon.Feature.SimilarImageSearch.Models
{
    public class SISResults : SearchResultItem
    {
        [IndexField("sis_dom_table")]
        public string ImageDOMTable { get; set; }
    }
}