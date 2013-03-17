namespace WebFeatures.Infrastructure
{
    using System;
    using System.ComponentModel.Composition;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false), MetadataAttribute]
    public class WebFeatureExportAttribute : ExportAttribute, IFeatureMetadata
    {
        public WebFeatureExportAttribute(string featureName) : base(typeof(IWebFeature))
        {
            FeatureName = featureName;
        }

        public string FeatureName { get; private set; }
    }

    public interface IFeatureMetadata
    {
        string FeatureName { get; }
    }
}