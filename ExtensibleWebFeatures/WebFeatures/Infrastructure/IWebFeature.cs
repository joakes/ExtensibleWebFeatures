namespace WebFeatures.Infrastructure
{
    using System.Collections.Generic;

    public interface IWebFeature
    {
        string PlaceHolderName { get; }
        string Name { get; }
        List<string> Pages { get; }
        string ResourcePath { get; }

        T GetProperty<T>(string propertyName);
    }
}
