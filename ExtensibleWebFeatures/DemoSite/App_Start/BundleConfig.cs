using System.Text;
using WebFeatures.Infrastructure;

namespace DemoSite.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Optimization;

    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/Content/less").Include("~/Content/start.less"));
        }
    }

    public static class BundleExtensions
    {
        public static void CopyLessFiles(Type type, string directoryVirtualPath, string importsLessFile)
        {
            var lessFileNames = CopyLessResourcesToServer(type, directoryVirtualPath);
            AppendLessFilesToImportsScript(directoryVirtualPath, importsLessFile, lessFileNames);
        }

        private static IEnumerable<string> CopyLessResourcesToServer(Type type, string directoryVirtualPath)
        {
            var assembly = type.Assembly;
            var resourceNames = assembly.GetManifestResourceNames()
                                        .Where(WebFeatureManager.IsResourceRequiredForActiveFeature)
                                        .ToList();

            var serverDir = BundleTable.MapPathMethod(directoryVirtualPath);
            if (!Directory.Exists(serverDir))
            {
                Directory.CreateDirectory(serverDir);
            }

            foreach (var resourceName in resourceNames)
            {
                var path = Path.Combine(directoryVirtualPath, resourceName);
                var fullPath = BundleTable.MapPathMethod(path);

                using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
                {
                    var manifestResourceStream = assembly.GetManifestResourceStream(resourceName);
                    if (manifestResourceStream != null)
                    {
                        manifestResourceStream.CopyTo(fileStream);
                    }
                }
            }

            return resourceNames;
        }

        private static void AppendLessFilesToImportsScript(string directoryVirtualPath, string importsLessFile, IEnumerable<string> lessFileNames)
        {
            var virtualPath = Path.Combine(directoryVirtualPath, importsLessFile);
            var serverPath = BundleTable.MapPathMethod(virtualPath);

            var imports = new StringBuilder();
            foreach (var lessFile in lessFileNames)
            {
                imports.AppendFormat("@import \"{0}\";{1}", lessFile, Environment.NewLine);
            }

            File.WriteAllText(serverPath, imports.ToString());
        }
    }
}