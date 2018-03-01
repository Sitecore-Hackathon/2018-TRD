using Sitecore.Data;
using Sitecore.Data.Comparers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.CustomForms;
using Sitecore.Globalization;
using Sitecore.Layouts;
using Sitecore.Pipelines;
using Sitecore.Pipelines.ExpandInitialFieldValue;
using Sitecore.Resources;
using Sitecore.SecurityModel;
using Sitecore.Shell.Applications.Dialogs.SortContent;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.SitecoreExtensions.Commands
{
	[Serializable]
	public class DeleteDatasourceChildren : WebEditCommand
	{
		public override void Execute(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");
			string str = context.Parameters.Get("itemid");
			if (string.IsNullOrEmpty(str))
			{
				str = context.Parameters.Get("id");
			}
			NameValueCollection values = new NameValueCollection();
			if (!string.IsNullOrEmpty(str))
			{
				values.Set("itemid", str);
				Context.ClientPage.Start(this, "Run", values);
			}
			else
			{
				Item item = (context.Items.Length > 0) ? context.Items[0] : null;
				if (item == null)
				{
					SheerResponse.Alert("Item \"{0}\" not found.", new string[0]);
				}
				else
				{
					string referenceId = context.Parameters.Get("referenceid");
					if (string.IsNullOrEmpty(referenceId))
					{
						values.Set("itemid", item.ID.ToString());
						Context.ClientPage.Start("Run", values);
					}
					else
					{
						Item item2 = this.GetDatasourceItem(referenceId, item) ?? item;
						values.Set("itemid", item2.ID.ToString());
						Context.ClientPage.Start(this, "Run", values);
					}
				}
			}
		}

		private Item GetDatasourceItem(string referenceId, Item item)
		{
			Assert.ArgumentNotNull(referenceId, "referenceId");
			Assert.ArgumentNotNull(item, "item");
			string str2 = WebEditUtil.ConvertJSONLayoutToXML(WebUtil.GetFormValue("scLayout"));
			Assert.IsNotNull(str2, "xmlLayout");
			LayoutDefinition definition = LayoutDefinition.Parse(str2);
			ID clientDeviceId = WebEditUtil.GetClientDeviceId();
			Assert.IsNotNull(clientDeviceId, "deviceId");
			RenderingDefinition renderingByUniqueId = definition.GetDevice(clientDeviceId.ToString()).GetRenderingByUniqueId(referenceId);
			Assert.IsNotNull(renderingByUniqueId, "rendering");
			RenderingReference reference = new RenderingReference(renderingByUniqueId, item.Language, item.Database);
			if (!string.IsNullOrEmpty(reference.Settings.DataSource))
			{
				return item.Database.GetItem(reference.Settings.DataSource);
			}
			return null;
		}

		protected virtual DeleteContentOptions GetOptions(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			string str = args.Parameters.Get("itemid");
			Language language = WebEditUtil.GetClientContentLanguage() ?? Context.Language;
			Item item = Client.ContentDatabase.GetItem(str, language);
			Assert.IsNotNull(item, "item");
			return new DeleteContentOptions(item);
		}

		protected virtual void Run(ClientPipelineArgs args)
		{

			Assert.ArgumentNotNull(args, "args");
			if (SheerResponse.CheckModified())
			{
				if (args.IsPostBack)
				{
					if (args.HasResult)
					{
						WebEditCommand.Reload();
					}
				}
				else
				{
					SheerResponse.ShowModalDialog(this.GetOptions(args).ToUrlString().ToString(), true);
					args.WaitForPostBack(true);
				}
			}
		}
	}
}