using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHackathon.Feature.SimilarImageSearch.Models
{
    public class SISQuadrant:IComparable<SISQuadrant>
    {
        public SISQuadrant(List<int> quadrantDomTable)
        {
            QuadrantDomTable = quadrantDomTable;
        }

        public List<int> QuadrantDomTable { get; private set; }

        public int CompareTo(SISQuadrant other)
        {
            int total = 0;
            for (int i = 0; i < QuadrantDomTable.Count; i++)
            {
                var diff = (other.QuadrantDomTable[i] - QuadrantDomTable[i]);
                total += diff * diff;
            }

            return total;
        }
    }
}