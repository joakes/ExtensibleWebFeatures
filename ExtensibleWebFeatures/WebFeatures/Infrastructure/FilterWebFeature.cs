namespace WebFeatures.Infrastructure
{
    using System.ComponentModel.Composition;
    using System.Text.RegularExpressions;

    [Export(typeof(IFilterWebFeature))]
    public class FilterWebFeature : IFilterWebFeature
    {
        public IWebFeature GetSpecificWebFeature(string presenterTypeName)
        {
            var featureName = Regex.Replace(presenterTypeName, "Presenter", string.Empty);
            return WebFeatureManager.GetWebFeature(featureName);
        }
    }

    public interface IFilterWebFeature
    {
        IWebFeature GetSpecificWebFeature(string featureName);
    }
}
