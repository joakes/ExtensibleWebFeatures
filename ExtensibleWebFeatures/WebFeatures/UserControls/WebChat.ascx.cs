using System;

namespace WebFeatures.UserControls
{
    using Infrastructure;

    public partial class WebChat : BaseWebFeatureControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var value = WebFeature.GetProperty<string>("Url");
        }
    }
}