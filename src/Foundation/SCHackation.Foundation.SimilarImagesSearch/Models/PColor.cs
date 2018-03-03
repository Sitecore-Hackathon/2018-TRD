using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHackathon.Foundation.SimilarImagesSearch.Models
{
	public class PColor
	{
		public PColor(EnumPColors color, Color sysColor)
		{
			Color = color;
			SystemColor = sysColor;
		}

		public EnumPColors Color { get; private set; }

		public Color SystemColor { get; private set; }

		public static class WebColors
		{
			public static PColor White => new PColor(EnumPColors.White, System.Drawing.Color.FromArgb(255, 255, 255));
			public static PColor Silver => new PColor(EnumPColors.Silver, System.Drawing.Color.FromArgb(192, 192, 192));
			public static PColor Gray => new PColor(EnumPColors.Gray, System.Drawing.Color.FromArgb(128, 128, 128));
			public static PColor Black => new PColor(EnumPColors.Black, System.Drawing.Color.FromArgb(0, 0, 0));
			public static PColor Red => new PColor(EnumPColors.Red, System.Drawing.Color.FromArgb(255, 0, 0));
			public static PColor Maroon => new PColor(EnumPColors.Maroon, System.Drawing.Color.FromArgb(128, 0, 0));
			public static PColor Yellow => new PColor(EnumPColors.Yellow, System.Drawing.Color.FromArgb(255, 255, 0));
			public static PColor Olive => new PColor(EnumPColors.Olive, System.Drawing.Color.FromArgb(128, 128, 0));
			public static PColor Lime => new PColor(EnumPColors.Lime, System.Drawing.Color.FromArgb(0, 255, 0));
			public static PColor Green => new PColor(EnumPColors.Green, System.Drawing.Color.FromArgb(0, 125, 0));
			public static PColor Aqua => new PColor(EnumPColors.Aqua, System.Drawing.Color.FromArgb(0, 255, 255));
			public static PColor Teal => new PColor(EnumPColors.Teal, System.Drawing.Color.FromArgb(0, 128, 128));
			public static PColor Blue => new PColor(EnumPColors.Blue, System.Drawing.Color.FromArgb(0, 0, 255));
			public static PColor Navy => new PColor(EnumPColors.Navy, System.Drawing.Color.FromArgb(0, 0, 128));
			public static PColor Fuchsia => new PColor(EnumPColors.Fuchsia, System.Drawing.Color.FromArgb(255, 0, 255));
			public static PColor Purple => new PColor(EnumPColors.Purple, System.Drawing.Color.FromArgb(128, 0, 128));

			public static List<PColor> All => new List<PColor> { White, Silver, Gray, Black, Red, Maroon, Yellow, Olive, Lime, Green, Aqua, Teal, Blue, Navy, Fuchsia, Purple };
		}
	}
}
