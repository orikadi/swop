using swop.Requests;
using swop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace swop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CreatePictureDirectories();
            Load_RequestHandler();


        }

        private void CreatePictureDirectories()
        {
            System.IO.Directory.CreateDirectory(Server.MapPath("~/Uploads/UserPictures/"));
            System.IO.Directory.CreateDirectory(Server.MapPath("~/Uploads/ApartmentPictures/"));
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (Session["Logged"] == null)
            {
                Session["Logged"] = false;
            }
        }

        //load requests from db
        //get instance of request handler and load all of the requests into the handler without updating the database
        private void Load_RequestHandler()
        {
            //add corequest usage boolean
            SwopContext db = new SwopContext();
            RequestHandler requestHandler = RequestHandler.Instance;
            List<Request> reqList = new List<Request>();
            reqList = (from req in db.Requests where req.State == 0 select req).ToList();
            foreach (Request r in reqList)
            {
                requestHandler.AddRequest(r, false);
            }
        }
    }
}
