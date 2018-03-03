using System.Linq;
using System.IO;
using SCHackathon.Feature.SimilarImageSearch.Models;
using SCHackathon.Foundation.SimilarImagesSearch;
using System.Collections.Generic;
using SCHackathon.Feature.SimilarImageSearch.Repositories;
using Sitecore.Foundation.DependencyInjection;

namespace SCHackathon.Feature.SimilarImageSearch.Services
{
    [Service(typeof(ISISImageSearch))]
    public class SISImageSearch : ISISImageSearch
    {
        ISISRepository _repo;
        public SISImageSearch(ISISRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Parses the input stream and returns the most similar images according to Color Dominance
        /// </summary>
        /// <param name="sourceImageStream"></param>
        /// <returns></returns>
        public List<SISImage> GetSimilarImages(Stream sourceImageStream, int pageSize, int pageNumber)
        {
            if (sourceImageStream == null)
                return null;
            var quadResults = sourceImageStream.ExtractQuadrantsColorDominance();

            //Calculate color dominance for each quadrant
            SISQuadrant q1 = new SISQuadrant(quadResults[0].Select(s => s.PercentageDiff).ToList());
            SISQuadrant q2 = new SISQuadrant(quadResults[1].Select(s => s.PercentageDiff).ToList());
            SISQuadrant q3 = new SISQuadrant(quadResults[2].Select(s => s.PercentageDiff).ToList());
            SISQuadrant q4 = new SISQuadrant(quadResults[3].Select(s => s.PercentageDiff).ToList());
            SISQuadrant qFullImage = new SISQuadrant(quadResults[4].Select(s => s.PercentageDiff).ToList());

            SISImage image = new SISImage(null, q1, q2, q3, q4, qFullImage);

            //Get the stored images data
            var savedImages = _repo.GetImagesData();

            //DO the the comparing and ordering
            return savedImages.OrderBy(o => o.CompareTo(image)).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
        }
    }
}