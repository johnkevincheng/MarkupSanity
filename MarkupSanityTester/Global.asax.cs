using System;
using System.Collections.Generic;

namespace MarkupSanityTester
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            MarkupSanity.Configure.CustomWhitelistedTags = new List<String>() { "i", "em", "strong" };
            MarkupSanity.Configure.CustomWhitelistedAttributes = new List<String>() { "id" };
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}