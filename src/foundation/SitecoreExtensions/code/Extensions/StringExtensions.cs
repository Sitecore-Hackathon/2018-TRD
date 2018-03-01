namespace Sitecore.Foundation.SitecoreExtensions.Extensions
{
	using HtmlAgilityPack;
	using System.Linq;
	using System.Text.RegularExpressions;

	public static class StringExtensions
	{
		public static string Humanize(this string input)
		{
			return Regex.Replace(input, "(\\B[A-Z])", " $1");
		}

		public static string ToCssUrlValue(this string url)
		{
			return string.IsNullOrWhiteSpace(url) ? "none" : $"url('{url}')";
		}

		/// <summary>
		/// Trims the rich text.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="maxLength">The maximum length.</param>
		/// <returns></returns>
		public static string TrimRichText(this string input, int maxLength, string postText = "")
		{
			if (string.IsNullOrWhiteSpace(input))
				return string.Empty;

			var doc = new HtmlDocument();
			doc.LoadHtml(input);

			var innerText = doc.DocumentNode.InnerText;

			return innerText.Substring(0, innerText.Length > maxLength ? maxLength : innerText.Length) + (innerText.Length > maxLength ? postText : string.Empty);
		}
	}
}