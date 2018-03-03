using System;
using System.Linq;

namespace SCHackathon.Foundation.SimilarImagesSearch.Models
{
	public class PDiff : IComparable<PDiff>
	{
		public PDiff(EnumPColors color, double colorCount, double totalCount)
		{
			Color = PColor.WebColors.All.FirstOrDefault(f => f.Color == color);
			PercentageDiff = Convert.ToInt32((colorCount / totalCount) * 100);
		}

		public PColor Color { get; private set; }
		public int PercentageDiff { get; private set; }

		public int CompareTo(PDiff other)
		{
			if (this == null || other == null)
				return -1;

			if (this.PercentageDiff > other.PercentageDiff)
				return -1;

			if (this.PercentageDiff < other.PercentageDiff)
				return 1;

			return 0;
		}
	}
}
