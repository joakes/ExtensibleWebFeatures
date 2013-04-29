namespace WebFeatures.Infrastructure
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ScriptReference : WebControl
    {
        public string Path { get; set; }
        public bool IsEmbedded { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (string.IsNullOrEmpty(Path))
            {
                throw new ArgumentException("Script property must have a value");
            }

            var manager = ScriptManager.GetCurrent(Page);
            if (manager == null)
            {
                throw new ArgumentException("No script manager found for the current page. Have you added a ScriptManager to the master page?");
            }

            System.Web.UI.ScriptReference scriptReference;
            if (IsEmbedded)
            {
                var parentBaseType = Parent.GetType().BaseType;
                var scriptResourcePrefix = parentBaseType.Namespace;
                var scriptResourcePath = string.Format("{0}.{1}", scriptResourcePrefix, Path);
                var assemblyName = parentBaseType.Assembly.GetName().Name;
                scriptReference = new System.Web.UI.ScriptReference(scriptResourcePath, assemblyName);
            }
            else
            {
                scriptReference = new System.Web.UI.ScriptReference(Path);
            }
            
            manager.Scripts.Add(scriptReference);
        }
    }
}