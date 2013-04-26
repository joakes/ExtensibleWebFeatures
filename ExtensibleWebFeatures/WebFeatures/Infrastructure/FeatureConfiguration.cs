namespace WebFeatures.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Xml.Linq;

    [Export(typeof(IFeatureConfiguration))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class FeatureConfiguration : IFeatureConfiguration
    {
        private readonly FeatureConfig _featureConfig;
        private readonly string _configFileName;

        public FeatureConfiguration()
        {
            _configFileName = Assembly.GetExecutingAssembly().GetName().Name.ToLowerInvariant() + ".xml";
            _featureConfig = InitialiseFeatureConfigFromXml();
        }

        public bool IsFeatureEnabled(string featureName)
        {
            var feature = _featureConfig.Features.FirstOrDefault(x => x.Name == featureName);
            return feature != null && feature.Enabled;
        }

        public T GetProperty<T>(string featureName, string propertyName)
        {
            var feature = _featureConfig.Features.FirstOrDefault(x => x.Name == featureName);

            if (feature == null || !feature.Properties.Any())
            {
                return default(T);
            }

            var val = feature.Properties.FirstOrDefault(x => x.Key == propertyName).Value;
            if (string.IsNullOrEmpty(val))
            {
                return default(T);
            }

            try
            {
                return (T)Convert.ChangeType(val, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        private FeatureConfig InitialiseFeatureConfigFromXml()
        {
            var config = new FeatureConfig();
            var configFilePath = HttpContext.Current.Server.MapPath(@"./bin/" + _configFileName);
            var doc = XDocument.Load(configFilePath);

            var root = doc.Root;
            var features = from feature in root.Elements("feature")
                           let props = feature.Descendants("property")
                           select CreateFeatureFromXDoc(feature, props);
            
            config.Features = features.ToList();
            return config;
        }

        private Feature CreateFeatureFromXDoc(XElement featureNode, IEnumerable<XElement> propertiesNode)
        {
            Func<string, bool> toBool = val => 
                {
                    bool result;
                    return bool.TryParse(val, out result) && result;
                };

            var properties = propertiesNode.Select(x =>
                                 new KeyValuePair<string, string>(
                                    x.Attribute("key").Value,
                                    x.Attribute("value").Value)).ToList();

            var feature = new Feature
            {
                Name = featureNode.Attribute("name").Value,
                Enabled = toBool(featureNode.Attribute("enabled").Value),
                Properties = properties
            };
            
            return feature;
        }
    }

    public class FeatureConfig
    {
        public FeatureConfig()
        {
            Features = new List<Feature>();
        }

        public List<Feature> Features  { get; set; }
    }

    public class Feature
    {
        public Feature()
        {
            Properties = new List<KeyValuePair<string, string>>();
        }

        public string Name { get; set; }
        public bool Enabled { get; set; }
        public List<KeyValuePair<string, string>> Properties { get; set; }
    }

    public interface IFeatureConfiguration
    {
        bool IsFeatureEnabled(string featureName);
        T GetProperty<T>(string featureName, string propertyName);
    }
}