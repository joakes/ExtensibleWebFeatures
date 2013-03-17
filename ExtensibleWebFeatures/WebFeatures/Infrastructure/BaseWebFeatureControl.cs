using System;

namespace WebFeatures.Infrastructure
{
    using System.Web.UI;

    public abstract class BaseWebFeatureControl : UserControl
    {
        protected IWebFeature WebFeature;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var type = GetType();
            WebFeature = WebFeatureManager.GetWebFeature(type.BaseType.Name);
            // todo: 2. put this into a Presenter
        }
    }

    public class WebChatPresenter
    {
        public WebChatPresenter()
        {
            
        }
    }

    public interface IWebChatView
    {
        
    }
}