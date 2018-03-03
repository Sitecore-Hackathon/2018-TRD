using SCHackathon.Feature.SimilarImageSearch.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCHackathon.Feature.SimilarImageSearch.Models
{
    public class SISImage : IComparable<SISImage>
    {
        public SISImage(ID baseItemID, SISQuadrant quad1, SISQuadrant quad2, SISQuadrant quad3, SISQuadrant quad4, SISQuadrant fullImage)
        {
            BaseItemID = baseItemID;
            Quadrant1 = quad1;
            Quadrant2 = quad2;
            Quadrant3 = quad3;
            Quadrant4 = quad4;
            FullImage = fullImage;
        }

        public ID BaseItemID { get; private set; }

        public SISQuadrant Quadrant1 { get; private set; }
        public SISQuadrant Quadrant2 { get; private set; }
        public SISQuadrant Quadrant3 { get; private set; }
        public SISQuadrant Quadrant4 { get; private set; }
        public SISQuadrant FullImage { get; private set; }

        public int CompareTo(SISImage other)
        {
            var avg = (double)(Quadrant1.CompareTo(other.Quadrant1) +
                (double)Quadrant2.CompareTo(other.Quadrant2) +
                (double)Quadrant3.CompareTo(other.Quadrant3) +
                (double)Quadrant4.CompareTo(other.Quadrant4));


            //return FullImage.CompareTo(other.FullImage); // +
            return (int)Math.Abs(avg - Quadrant1.CompareTo(other.Quadrant1)) ^ 2 +
                (int)Math.Abs(avg - Quadrant2.CompareTo(other.Quadrant2)) ^ 2 +
                (int)Math.Abs(avg - Quadrant3.CompareTo(other.Quadrant3)) ^ 2 +
                (int)Math.Abs(avg - Quadrant4.CompareTo(other.Quadrant4)) ^ 2;
        }
    }
}