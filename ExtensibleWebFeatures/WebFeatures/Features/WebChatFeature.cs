namespace WebFeatures.Features
{
    using WebFeatures.Infrastructure;

    [WebFeatureExport("WebChat")]
    public class WebChatFeature : BaseWebFeature
    {
        protected override void ConfigureFeature()
        {
            Pages.Add("Default.aspx");
            PlaceHolderName = "Menu";
        }
    }
}