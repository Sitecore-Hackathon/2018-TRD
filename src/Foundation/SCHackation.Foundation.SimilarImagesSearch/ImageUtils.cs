using ColorMine.ColorSpaces;
using ColorMine.ColorSpaces.Comparisons;
using SCHackathon.Foundation.SimilarImagesSearch.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace SCHackathon.Foundation.SimilarImagesSearch
{
	public static class ImageUtils
	{
		/// <summary>
		/// Returns the custom color nearest to the System color input
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static PColor FindNearestColor(this Color input)
		{
			PColor resultMSColor = PColor.WebColors.White;
			var resultDistance = 255.0;

			//Create ColorMine RGB and LAB variables for comparisons
			Rgb inColoRgb = new Rgb { R = input.R, G = input.G, B = input.B };
			Lab inColorLab = inColoRgb.To<Lab>();

			foreach (var pcolor in PColor.WebColors.All)
			{
				var comparerLabColor = new CieDe2000Comparison();
				var mineColorRgb = new Rgb { R = pcolor.SystemColor.R, G = pcolor.SystemColor.G, B = pcolor.SystemColor.B };
				Lab mineColorLab = mineColorRgb.To<Lab>();

				//Comare colors and check if closer than previous closest
				var currentDistence = inColoRgb.Compare(mineColorLab, comparerLabColor);
				if (currentDistence == 0)
				{
					resultMSColor = pcolor;
					break;
				}
				else if (currentDistence < resultDistance)
				{
					resultMSColor = pcolor;
					resultDistance = currentDistence;
				}
			}

			return resultMSColor;
		}
		
		/// <summary>
		/// Extracts the dominant colors table from the input image stream for each image quadrant
		/// </summary>
		/// <param name="imageStream"></param>
		/// <returns></returns>
		public static List<PDiff>[] ExtractQuadrantsColorDominance(this Stream imageStream)
		{
			Bitmap bit = new Bitmap(imageStream);

			bit = bit.ResizeImage(128, 128);

			int halfWidth = (bit.Width - 1) / 2;
			int halfHeight = (bit.Height - 1) / 2;
			var bit1 = bit.Clone(new Rectangle(0, 0, halfWidth, halfHeight), PixelFormat.Format64bppArgb);
			var bit2 = bit.Clone(new Rectangle(halfWidth, 0, halfWidth, halfHeight), PixelFormat.Format64bppArgb);
			var bit3 = bit.Clone(new Rectangle(0, halfHeight, halfWidth, halfHeight), PixelFormat.Format64bppArgb);
			var bit4 = bit.Clone(new Rectangle(halfWidth, halfHeight, halfWidth, halfHeight), PixelFormat.Format64bppArgb);

			return new[] { bit1.ExtractColorDominance(), bit2.ExtractColorDominance(), bit3.ExtractColorDominance(), bit4.ExtractColorDominance(), bit.ExtractColorDominance() };
		}

		private static List<PDiff> ExtractColorDominance(this Bitmap bitm)
		{
			var colorDom = new List<PDiff>();

			if (bitm == null)
				throw new ArgumentNullException("bitm");

			Dictionary<EnumPColors, int> calc = PColor.WebColors.All.ToDictionary(k => k.Color, e => 0);

			int totalCount = 0;
			for (var col = 0; col < bitm.Width; col++)
			{
				for (int row = 0; row < bitm.Height; row++)
				{
					//Get the current pixel's nearest PColor
					PColor closestPColor = bitm.GetPixel(col, row).FindNearestColor();

					switch (closestPColor.Color)
					{
						case EnumPColors.Aqua:
							calc[EnumPColors.Aqua]++;
							break;
						case EnumPColors.Black:
							calc[EnumPColors.Black]++;
							break;
						case EnumPColors.Blue:
							calc[EnumPColors.Blue]++;
							break;
						case EnumPColors.Fuchsia:
							calc[EnumPColors.Fuchsia]++;
							break;
						case EnumPColors.Gray:
							calc[EnumPColors.Gray]++;
							break;
						case EnumPColors.Green:
							calc[EnumPColors.Green]++;
							break;
						case EnumPColors.Lime:
							calc[EnumPColors.Lime]++;
							break;
						case EnumPColors.Maroon:
							calc[EnumPColors.Maroon]++;
							break;
						case EnumPColors.Navy:
							calc[EnumPColors.Navy]++;
							break;
						case EnumPColors.Olive:
							calc[EnumPColors.Olive]++;
							break;
						case EnumPColors.Purple:
							calc[EnumPColors.Purple]++;
							break;
						case EnumPColors.Red:
							calc[EnumPColors.Red]++;
							break;
						case EnumPColors.Silver:
							calc[EnumPColors.Silver]++;
							break;
						case EnumPColors.Teal:
							calc[EnumPColors.Teal]++;
							break;
						case EnumPColors.White:
							calc[EnumPColors.White]++;
							break;
						case EnumPColors.Yellow:
							calc[EnumPColors.Yellow]++;
							break;
					}

					totalCount++;
				}
			}

			colorDom.AddRange(calc.Select(s => new PDiff(s.Key, s.Value, totalCount)));
			//colorDom.Sort();

			return colorDom;
		}

		/// <summary>
		/// Resizes images into input sizes
		/// </summary>
		/// <param name="image"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Bitmap ResizeImage(this Bitmap image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}
	}
}
