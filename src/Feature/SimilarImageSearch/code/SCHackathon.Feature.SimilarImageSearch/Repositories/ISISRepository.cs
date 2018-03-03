using SCHackathon.Feature.SimilarImageSearch.Models;
using System.Collections.Generic;

namespace SCHackathon.Feature.SimilarImageSearch.Repositories
{
    public interface ISISRepository
    {
        List<SISImage> GetImagesData();
    }
}