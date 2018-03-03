using SCHackathon.Feature.SimilarImageSearch.Models;
using Sitecore.ContentSearch;
using Sitecore.Foundation.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHackathon.Feature.SimilarImageSearch.Repositories
{
    [Service(typeof(ISISRepository))]
    public class SISRepository : ISISRepository
    {
        private const string indexName = "sitecore_sis_master_index";
        public List<SISImage> GetImagesData()
        {
            if (HttpContext.Current.Cache["SISImageResults"] == null)
            {
                //if cache is empty, get images data from index and cache
                using (var ctx = ContentSearchManager.GetIndex(indexName).CreateSearchContext())
                {
                    var query = ctx.GetQueryable<SISResults>();

                    var qresults = query.ToList();

                    HttpContext.Current.Cache["SISImageResults"] = ParseResults(qresults);
                }
            }

            return HttpContext.Current.Cache["SISImageResults"] as List<SISImage>;
        }

        private List<SISImage> ParseResults(List<SISResults> results)
        {
            //Parse indexed data (string concatenated data) into models
            var parsedResults = results.Where(w => w.ImageDOMTable != null).Select((s) =>
            {
                var quadSplits = s.ImageDOMTable.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Select((t) =>
                {
                    var splitted = t.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    return new SISQuadrant(splitted.Select(i => int.Parse(i)).ToList());
                }).ToArray();

                return new SISImage(s.ItemId, quadSplits[0], quadSplits[1], quadSplits[2], quadSplits[3], quadSplits[4]);
            }).ToList();

            return parsedResults;
        }
    }
}