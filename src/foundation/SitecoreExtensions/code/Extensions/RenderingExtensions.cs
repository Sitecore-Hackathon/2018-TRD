namespace Sitecore.Foundation.SitecoreExtensions.Extensions
{
	using System;
	using Sitecore.Mvc.Presentation;
	using Sitecore.Resources.Media;
	using Sitecore.Xml;
	using Data.Fields;
	using Sitecore.StringExtensions;
	using Layouts;
	using Data.Items;
	public static class RenderingExtensions
	{
		public static int GetIntegerParameter(this Rendering rendering, string parameterName, int defaultValue = 0)
		{
			if (rendering == null)
			{
				throw new ArgumentNullException(nameof(rendering));
			}

			var parameter = rendering.Parameters[parameterName];
			if (string.IsNullOrEmpty(parameter))
			{
				return defaultValue;
			}

			int returnValue;
			return !int.TryParse(parameter, out returnValue) ? defaultValue : returnValue;
		}

		public static bool GetUseStaticPlaceholderNames([NotNull] this Rendering rendering)
		{
			return MainUtil.GetBool(rendering.Parameters[Constants.DynamicPlaceholdersLayoutParameters.UseStaticPlaceholderNames], false);
		}

		public static void MergeLayouts([NotNull]this Item item)
		{
			//Grab the field that contains the layout
			var layoutField = new LayoutField(item.Fields[Sitecore.FieldIDs.LayoutField]);

			//Grab the field that contains the final layout
			var finalLayoutField = new LayoutField(item.Fields[Sitecore.FieldIDs.FinalLayoutField]);

			if (layoutField == null)
				throw new Exception("Couldn't find layout on: {0}".FormatWith(item.Name));

			if (finalLayoutField == null)
				throw new Exception("Couldn't find final layout on: {0}".FormatWith(item.Name));

			//If we don't have a final layout delta, we're good!
			if (string.IsNullOrWhiteSpace(finalLayoutField.Value))
			{
				return;
			}

			var finalLayoutDefinition = LayoutDefinition.Parse(finalLayoutField.Value);

			using (new EditContext(item))
			{
				layoutField.Value = finalLayoutDefinition.ToXml();
				item.Fields["__Final Renderings"].Reset();
				item.Editing.AcceptChanges();
			}
		}

		public static void MoveLayouts([NotNull]this Item sourceItem, Item targetItem)
		{
			////Grab the field that contains the layout
			//var layoutField = new LayoutField(sourceItem.Fields[Sitecore.FieldIDs.LayoutField]);

			//Grab the field that contains the final layout
			var finalLayoutField = new LayoutField(sourceItem.Fields[Sitecore.FieldIDs.FinalLayoutField]);

			//if (layoutField == null)
			//	throw new Exception("Couldn't find layout on: {0}".FormatWith(sourceItem.Name));

			if (finalLayoutField == null)
				throw new Exception("Couldn't find final layout on: {0}".FormatWith(sourceItem.Name));

			////If we don't have a final layout delta, we're good!
			//if (string.IsNullOrWhiteSpace(finalLayoutField.Value))
			//{
			//	return;
			//}

			//Grab the field that contains the layout
			var targetLayoutField = new LayoutField(targetItem.Fields[Sitecore.FieldIDs.LayoutField]);

			//Grab the field that contains the final layout
			var targetFinalLayoutField = new LayoutField(targetItem.Fields[Sitecore.FieldIDs.FinalLayoutField]);

			//if (targetLayoutField == null)
			//	throw new Exception("Couldn't find layout on: {0}".FormatWith(targetItem.Name));

			//if (targetFinalLayoutField == null)
			//	throw new Exception("Couldn't find final layout on: {0}".FormatWith(targetItem.Name));

			////If we don't have a final layout delta, we're good!
			//if (string.IsNullOrWhiteSpace(targetFinalLayoutField.Value))
			//{
			//	return;
			//}

			var targetFinalLayoutDefinition = LayoutDefinition.Parse(finalLayoutField.Value);

			using (new EditContext(targetItem))
			{
				targetFinalLayoutField.Value = targetFinalLayoutDefinition.ToXml();
				//targetItem.Fields["__Final Renderings"].Reset();
				targetItem.Editing.AcceptChanges();
			}
		}
	}
}