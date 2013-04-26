namespace WebFeatures.Infrastructure
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ScriptReference : WebControl
    {
        public string Script { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (string.IsNullOrEmpty(Script))
            {
                throw new ArgumentException("Script property must have a value");
            }

            var scriptResourcePrefix = Parent.GetType().BaseType.Namespace;
            var scriptResourcePath = string.Format("{0}.{1}", scriptResourcePrefix, Script);
            var assemblyName = GetType().Assembly.GetName().Name;

            var manager = ScriptManager.GetCurrent(Page);
            if (manager == null)
            {
                throw new ArgumentException("No script manager found for the current page. Have you added a ScriptManager to the master page?");
            }

            var scriptReference = new System.Web.UI.ScriptReference(scriptResourcePath, assemblyName);
            manager.Scripts.Add(scriptReference);

            // This gets the web resourece URL to the script
            // Page.ClientScript.GetWebResourceUrl(GetType(), scriptResourcePath);
        }
    }
}