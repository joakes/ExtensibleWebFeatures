namespace WebFeatures.Presenters
{
    using Views;
    
    public class WebChatPresenter : BasePresenter
    {
        private readonly IWebChatView _webChatView;

        public WebChatPresenter(IWebChatView webChatView)
        {
            _webChatView = webChatView;
        }

        public override void OnPageLoad(bool isPostBack)
        {
            _webChatView.ChatUrl = WebFeature.GetProperty<string>("Url");
        }
    }
}