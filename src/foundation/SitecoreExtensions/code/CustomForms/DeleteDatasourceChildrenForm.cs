using Sitecore.Data;
using Sitecore.Data.Comparers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Pipelines;
using Sitecore.Pipelines.ExpandInitialFieldValue;
using Sitecore.Resources;
using Sitecore.SecurityModel;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.SitecoreExtensions.CustomForms
{
	public class DeleteDatasourceChildrenForm : DialogForm
	{
		private bool expandStandardValuesTokens;
		protected Scrollbox MainContainer;
		//private string sortBy;

		private Item[] GetItems(Item item, string query)
		{

			Item[] itemArray;
			Assert.ArgumentNotNull(item, "item");
			Assert.IsNotNullOrEmpty(query, "query");
			try
			{
				using (new LanguageSwitcher(item.Language))
				{
					if (query.StartsWith("fast:"))
					{
						return (item.Database.SelectItems(query.Substring(5)) ?? new Item[0]);
					}
					return (item.Axes.SelectItems(query) ?? new Item[0]);
				}
			}
			catch (Exception exception)
			{
				itemArray = new Item[0];
				Log.Error("Failed to execute query:" + query, exception, this);
			}
			return itemArray;
		}

		private string GetSortBy(Item item)
		{
			Assert.ArgumentNotNull(item, "item");
			return item.GetUIDisplayName();
		}

		private static bool IsEditable(Item item)
		{
			Assert.IsNotNull(item, "item");
			if ((!Context.IsAdministrator && item.Locking.IsLocked()) && !item.Locking.HasLock())
			{
				return false;
			}
			if (item.Appearance.ReadOnly)
			{
				return false;
			}
			return item.Access.CanWrite();
		}

		protected override void OnLoad(EventArgs e)
		{
			Assert.ArgumentNotNull(e, "e");
			base.OnLoad(e);
			if (!Context.ClientPage.IsEvent)
			{
				DeleteContentOptions options = DeleteContentOptions.Parse();
				//this.sortBy = options.SortBy;
				this.expandStandardValuesTokens = options.ExpandStandardValuesTokens;
				string contentQuery = "*";
				Assert.IsNotNullOrEmpty(contentQuery, "query");
				Item[] itemsToSort = this.GetItems(options.Item, contentQuery);
				Array.Sort<Item>(itemsToSort, new DefaultComparer());
				if (itemsToSort.Length < 1)
				{
					base.OK.Disabled = true;
				}
				else
				{
					this.MainContainer.Controls.Clear();
					this.MainContainer.InnerHtml = this.Render(itemsToSort);
				}
			}
		}

		protected override void OnOK(object sender, EventArgs args)
		{
			Assert.ArgumentNotNull(sender, "sender");
			Assert.ArgumentNotNull(args, "args");
			ListString str = new ListString(WebUtil.GetFormValue("deleteItem"));
			if (str.Count == 0)
			{
				base.OnOK(sender, args);
			}
			else
			{
				foreach (var item in str)
				{
					var itemToDelete = Sitecore.Context.ContentDatabase.GetItem(ID.Parse(item));

					using (new SecurityDisabler())
					{
						itemToDelete.Delete();
					}
				}

				SheerResponse.SetDialogValue("1");
				base.OnOK(sender, args);
			}
		}

		private string Render(IEnumerable<Item> items)
		{
			Assert.ArgumentNotNull(items, "items");
			System.Web.UI.HtmlTextWriter writer = new System.Web.UI.HtmlTextWriter(new StringWriter());
			writer.Write("<ul id='sort-list'>");
			foreach (Item item in items)
			{
				this.Render(writer, item);
			}
			writer.Write("</ul>");
			return writer.InnerWriter.ToString();
		}

		private void Render(System.Web.UI.HtmlTextWriter writer, Item item)
		{
			Assert.ArgumentNotNull(writer, "writer");
			Assert.ArgumentNotNull(item, "item");
			bool flag = IsEditable(item);
			string sortBy = this.GetSortBy(item);
			string str2 = !flag ? Translate.Text("You cannot edit this item because you do not have write access to it.") : sortBy;
			writer.Write("<li id='{0}' class='sort-item {1}' title='{2}'>", item.ID.ToShortID(), flag ? "editable" : "non-editable", str2);
			//writer.Write("<img src='/sitecore/shell/Themes/Standard/Images/draghandle9x15.png' class='drag-handle' />");
			writer.Write("<img src='{0}' class='item-icon' />", Images.GetThemedImageSource(item.Appearance.Icon, ImageDimension.id16x16));
			writer.Write("<span unselectable='on' class='item-name'>{0}</span>", StringUtil.Clip(sortBy, 40, true));
			writer.Write("</li>");
		}
	}

	public class DeleteContentOptions
	{
		public DeleteContentOptions(Sitecore.Data.Items.Item item)
		{
			Assert.ArgumentNotNull(item, "item");
			this.Item = item;
		}

		protected virtual string GetXmlControl()
		{
			return "Sitecore.Shell.Applications.Dialogs.DeleteContent";
		}

		public static DeleteContentOptions Parse()
		{
			ItemUri uri = ItemUri.ParseQueryString();
			Assert.IsNotNull(uri, "itemUri is null");
			Sitecore.Data.Items.Item item = Database.GetItem(uri);
			Assert.IsNotNull(item, "Item \"{0}\" not found", new object[] { uri });
			DeleteContentOptions options = new DeleteContentOptions(item);

			options.ExpandStandardValuesTokens = MainUtil.GetBool(WebUtil.GetQueryString("expandtokens"), false);
			return options;
		}

		public virtual UrlString ToUrlString()
		{
			Assert.IsNotNull(Context.Site, "context site");
			UrlString urlString = new UrlString(Context.Site.XmlControlPage);
			urlString["xmlcontrol"] = this.GetXmlControl();
			this.Item.Uri.AddToUrlString(urlString);
			//if (!string.IsNullOrEmpty(this.SortBy))
			//{
			//	urlString["sortby"] = this.SortBy;
			//}
			//if (!string.IsNullOrEmpty(this.ContentToSortQuery))
			//{
			//	urlString["contentquery"] = this.ContentToSortQuery;
			//}
			urlString["expandtokens"] = this.ExpandStandardValuesTokens ? "1" : "0";
			return Assert.ResultNotNull<UrlString>(urlString);
		}

		public bool ExpandStandardValuesTokens { get; set; }

		public Sitecore.Data.Items.Item Item { get; protected set; }
	}
}