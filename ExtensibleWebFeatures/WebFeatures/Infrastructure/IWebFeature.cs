using System.Collections.ObjectModel;

namespace WebFeatures.Infrastructure
{
    using System.Collections.Generic;

    public interface IWebFeature
    {
        string PlaceHolderName { get; }
        string Name { get; }
        List<string> Pages { get; }
        string ResourcePath { get; }
        ReadOnlyCollection<string> ResourceDependencies { get; }

        T GetProperty<T>(string propertyName);
    }
}
