using SCHackathon.Feature.SimilarImageSearch.Models;
using System.Collections.Generic;
using System.IO;

namespace SCHackathon.Feature.SimilarImageSearch.Services
{
    public interface ISISImageSearch
    {
        List<SISImage> GetSimilarImages(Stream sourceImageStream, int pageSize, int pageNumber);
    }
}