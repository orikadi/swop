﻿using System;
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
            Load_RequestHandler();
        }
        
        
        protected void Session_Start(object sender, EventArgs e)
        {
            if (Session["Logged"] == null)
            {
                Session["Logged"] = false;
            }
        }

        private void Load_RequestHandler()
        {
            //load requests from db
            //get instance of request handler and load all of the requests into the handler without updating the database

        }

    }
}
