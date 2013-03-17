using System;

namespace WebFeatures.UserControls
{
    using Presenters;
    using Views;
    
    public partial class WebChat : BaseWebFeatureControl, IWebChatView
    {
        private readonly WebChatPresenter _webChatPresenter;

        public WebChat()
        {
            _webChatPresenter = new WebChatPresenter(this);
        }
        
        private void Page_Load(object sender, EventArgs e)
        {
            _webChatPresenter.OnPageLoad(IsPostBack);
        }

        public string ChatUrl
        {
            set { ChatAnchor.Attributes["href"] = value; }
        }
    }
}