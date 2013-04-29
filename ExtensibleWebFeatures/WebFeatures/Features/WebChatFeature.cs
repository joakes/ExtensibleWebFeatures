namespace WebFeatures.Features
{
    using Infrastructure;

    [WebFeatureExport("WebChat")]
    public class WebChatFeature : BaseWebFeature
    {
        protected override void ConfigureFeature()
        {
            Pages.Add("Default.aspx");
            PlaceHolderName = "Menu";
            AddResourceDependency("webChat.less");
        }
    }
}