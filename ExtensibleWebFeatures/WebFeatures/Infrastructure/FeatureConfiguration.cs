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

        public FeatureConfig InitialiseFeatureConfigFromXml()
        {
            var config = new FeatureConfig();
            var configFilePath = HttpContext.Current.Server.MapPath(@"./bin/" + _configFileName);
            var doc = XDocument.Load(configFilePath);

            var root = doc.Root;
            var features = from feature in root.Elements("feature")
                           let props = feature.Descendants("property")
                           select CreateFeatureFromXDoc(feature, props);

            var menuGroups = root.Element("menugroups")
                                 .Elements("group")    
                                 .Select(CreateMenuGroupFromElement);

            config.Features = features.ToList();
            config.MenuGroups = menuGroups.ToList();
            return config;
        }

        private MenuGroup CreateMenuGroupFromElement(XElement grp)
        {
            var menuGroup = new MenuGroup
                                {
                                    CssClass = grp.Attribute("cssClass").Value,
                                    Name = grp.Attribute("name").Value,
                                    Position = int.Parse(grp.Attribute("position").Value)
                                };

            return menuGroup;
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
                Properties = properties,
                MenuItem = GetMenuItems(featureNode.Element("menu"))
            };
            
            return feature;
        }

        private MenuItem GetMenuItems(XElement element)
        {
            var item = new MenuItem
            {
                CssClass = element.Attribute("cssClass").Value,
                Name = element.Attribute("name").Value,
                Position = int.Parse(element.Attribute("position").Value),
                Group = element.Attribute("group").Value
            };

            return item;
        }
    }

    public class FeatureConfig
    {
        public FeatureConfig()
        {
            Features = new List<Feature>();
            MenuGroups = new List<MenuGroup>();
        }

        public List<MenuGroup> MenuGroups { get; set; }
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
        public MenuItem MenuItem { get; set; }
        public List<KeyValuePair<string, string>> Properties { get; set; }
    }

    public class MenuBase
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public string CssClass { get; set; }
    }
    
    public class MenuGroup : MenuBase { }
    public class MenuItem : MenuBase
    {
        public string Group { get; set; }
    }

    public interface IFeatureConfiguration
    {
        bool IsFeatureEnabled(string featureName);
        T GetProperty<T>(string featureName, string propertyName);
    }
}