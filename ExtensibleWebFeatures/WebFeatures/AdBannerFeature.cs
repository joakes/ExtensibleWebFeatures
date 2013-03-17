namespace WebFeatures
{
    using Infrastructure;

    [WebFeatureExport("AdBanner")]
    public class AdBannerFeature : BaseWebFeature
    {
        protected override void ConfigureFeature()
        {
            Pages.Add("Default.aspx");
            PlaceHolderName = "Menu";
        }
    }
}