using System;
using System.Web;

namespace DemoSite
{
    using System.Web.Optimization;
    using App_Start;
    using Infrastructure;

    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new AssemblyResourceProvider());
            BundleConfig.RegisterBundles(BundleTable.Bundles);
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