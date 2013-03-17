namespace WebFeatures.Infrastructure
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Reflection;

    public class WebFeatureManager
    {
        private static CompositionContainer container;
        private static IFeatureConfiguration featureConfig;

        static WebFeatureManager()
        {
            InitializeContainer();
        }

        private static void InitializeContainer()
        {
            var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            container = new CompositionContainer(assemblyCatalog);

            featureConfig = container.GetExportedValueOrDefault<IFeatureConfiguration>();
            if (featureConfig == null)
            {
                throw new CompositionException("Could not find feature configuration part");
            }
        }

        public static IWebFeature GetWebFeature(string featureName)
        {
            return container.GetExports<IWebFeature, IFeatureMetadata>()
                            .First(m => m.Metadata.FeatureName == featureName)
                            .Value;
        }

        public static IEnumerable<IWebFeature> GetActiveFeaturesForCurrentPage(string pageName, string placeholder)
        {
            var featuresThatAreOnThisPageAndThisSection = 
                container.GetExports<IWebFeature, IFeatureMetadata>()
                         .Where(m => featureConfig.IsFeatureEnabled(m.Metadata.FeatureName))
                         .Select(f => f.Value)
                         .Where(f => f.Pages.Contains(pageName))
                         .Where(f => f.PlaceHolderName == placeholder);

            return featuresThatAreOnThisPageAndThisSection;
        }
    }
}