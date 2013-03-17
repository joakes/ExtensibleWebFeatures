namespace WebFeatures.Infrastructure
{
    using System.ComponentModel.Composition;
    using System.Text.RegularExpressions;

    [Export(typeof(IFindWebFeature))]
    public class FindWebFeature : IFindWebFeature
    {
        public IWebFeature GetSpecificWebFeature(string presenterTypeName)
        {
            var featureName = Regex.Replace(presenterTypeName, "Presenter", string.Empty);
            return WebFeatureManager.GetWebFeature(featureName);
        }
    }

    public interface IFindWebFeature
    {
        IWebFeature GetSpecificWebFeature(string featureName);
    }
}
