namespace WebFeatures.Infrastructure
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public abstract class BaseWebFeature : IWebFeature, IPartImportsSatisfiedNotification
    {
        [Import]
        private IFeatureConfiguration _featureConfig;

        public const string MetaName = "MetaName";

        public void OnImportsSatisfied()
        {
            SetPropertiesByConvention();
            ConfigureFeature();
        }

        protected virtual void ConfigureFeature() { }

        public T GetProperty<T>(string propertyName)
        {
            return _featureConfig.GetProperty<T>(Name, propertyName);
        }
        
        private void SetPropertiesByConvention()
        {
            var type = GetType();
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var controlNamespace = type.Namespace + ".UserControls";
            var controlClassName = Regex.Replace(type.Name, "Feature", string.Empty);

            Pages = new List<string>();
            Name = controlClassName;
            ResourcePath = string.Format("/App_Resource/{0}.dll/{1}.{2}.ascx", assemblyName, controlNamespace, controlClassName);
        }

        public string PlaceHolderName { get; protected set; }
        public string Name { get; private set; }
        public List<string> Pages { get; private set; }
        public string ResourcePath { get; private set; }
    }
}