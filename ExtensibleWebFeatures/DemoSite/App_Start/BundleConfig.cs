namespace DemoSite.App_Start
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/Content/less").Include("~/Content/styles.less"));
        }
    }
}