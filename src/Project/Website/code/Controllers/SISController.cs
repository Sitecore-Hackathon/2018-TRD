using PagedList;
using SCH.Project.Web.Models;
using SCHackathon.Feature.SimilarImageSearch.Repositories;
using SCHackathon.Feature.SimilarImageSearch.Services;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCH.Project.Web.Controllers
{
    public class SISController : Controller
    {
        ISISImageSearch _imageSearcher;
        public SISController(/*ISISImageSearch imageSearcher*/)
        {
            _imageSearcher = new SISImageSearch(new SISRepository());
        }

        public ActionResult Search(HttpPostedFileBase file, int? pageNo, string renderingId)
        {
            System.IO.Stream stream = null;
            string searchScope = "/sitecore/Media Library";

            if (file != null)
            {
                Session["SIS_fileStream"] = file.InputStream;
                Session["SIS_pageNo"] = pageNo ?? 1;
                stream = (System.IO.Stream)Session["SIS_fileStream"];

                return Redirect(Request.UrlReferrer.ToString());
            }
            else if (Session["SIS_fileStream"] != null)
            {
                pageNo = int.Parse(Session["SIS_pageNo"].ToString());
                stream = (System.IO.Stream)Session["SIS_fileStream"];
            }

            try
            {
                if (stream == null)
                    return View();

                var tempResults = _imageSearcher.GetSimilarImages(stream, 10, pageNo ?? 1);

                List<SISViewModel> modelList = tempResults.Select(s => new SISViewModel(s.BaseItemID)).ToList();

                return View(modelList.ToPagedList(pageNo ?? 1, 5));
            }
            catch (Exception e)
            {
                // handle exception
                return Content("Error: " + e.Message);
            }
        }
    }
}