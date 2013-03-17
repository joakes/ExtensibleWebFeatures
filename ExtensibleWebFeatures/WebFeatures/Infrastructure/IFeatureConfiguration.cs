namespace WebFeatures.Infrastructure
{
    public interface IFeatureConfiguration 
    {
        bool IsFeatureEnabled(string featureName);
        T GetProperty<T>(string featureName, string propertyName);
    }
}