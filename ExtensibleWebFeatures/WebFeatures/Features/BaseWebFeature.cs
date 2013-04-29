using System.Collections.ObjectModel;
using System.Linq;

namespace WebFeatures.Features
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Infrastructure;

    public abstract class BaseWebFeature : IWebFeature, IPartImportsSatisfiedNotification
    {
        [Import]
        private IFeatureConfiguration _featureConfig;
        
        private List<string> _resourceDependencies;
        public const string MetaName = "MetaName";

        public string PlaceHolderName { get; protected set; }
        public string Name { get; private set; }
        public List<string> Pages { get; private set; }
        public string ResourcePath { get; private set; }
        
        public ReadOnlyCollection<string> ResourceDependencies
        {
            get { return _resourceDependencies == null ? null : _resourceDependencies.AsReadOnly(); }
        }

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

        public void AddResourceDependency(params string[] fileNames)
        {
            if (fileNames == null || !fileNames.Any())
            {
                return;
            }

            // var namespaceOfFeature = GetType().Namespace; // Use this line for NetTeller

            foreach (var fileName in fileNames)
            {
                var resourcePath = string.Format("WebFeatures.UserControls.{0}", fileName); // change this in NetTeller
                _resourceDependencies.Add(resourcePath);
            }
        }

        private void SetPropertiesByConvention()
        {
            var type = GetType();
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var controlNamespace = Regex.Replace(type.Namespace, Regex.Escape(".Features"), string.Empty) + ".UserControls";
            var controlClassName = Regex.Replace(type.Name, "Feature", string.Empty);


            Pages = new List<string>();
            _resourceDependencies = new List<string>();
            Name = controlClassName;
            ResourcePath = string.Format("/App_Resource/{0}.dll/{1}.{2}.ascx", assemblyName, controlNamespace, controlClassName);
        }
    }
}