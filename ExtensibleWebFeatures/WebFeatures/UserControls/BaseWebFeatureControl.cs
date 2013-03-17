namespace WebFeatures.UserControls
{
    using System;
    using System.Web.UI;
    using Infrastructure;

    public abstract class BaseWebFeatureControl : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            WebFeatureManager.ComposeParts(this);
        }
    }
}