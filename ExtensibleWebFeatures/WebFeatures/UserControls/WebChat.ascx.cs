using System;
using System.Web.UI;

namespace WebFeatures.UserControls
{
    using System.Web;
    using Presenters;
    using Views;
    
    public partial class WebChat : System.Web.UI.UserControl, IWebChatView
    {
        private readonly WebChatPresenter _webChatPresenter;

        public WebChat()
        {
            _webChatPresenter = new WebChatPresenter(this);
            // AddScript();
        }

        private void AddScript()
        {
            var page = HttpContext.Current.Handler as Page;
            if (page == null)
            {
                throw new ArgumentException("Script Manager not found");
            }

            var manager = ScriptManager.GetCurrent(page);
            if (manager != null)
            {
                var scriptReference = new ScriptReference("WebFeatures.Scripts.webChat.js", "WebFeatures");
                manager.Scripts.Add(scriptReference);
            }

            // var script = page.ClientScript.GetWebResourceUrl(typeof(WebChat), "WebFeatures.Scripts.webChat.js");
            // page.ClientScript.RegisterClientScriptInclude("WebChat", script);
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