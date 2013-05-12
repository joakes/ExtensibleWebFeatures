using System;

namespace DemoSite
{
    using Infrastructure;
    using WebFeatures.Infrastructure;

    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var activeFeatures = WebFeatureManager.GetActiveFeaturesForCurrentPage("Default.aspx", "Menu");

            foreach (var webFeature in activeFeatures)
            {
                var webChatControl = LoadControl(webFeature.ResourcePath);
                Menu.Controls.Add(webChatControl);   
            }

            var verticalMenu = new VerticalMenuBuilder().BuildMenu();
        }
    }
}