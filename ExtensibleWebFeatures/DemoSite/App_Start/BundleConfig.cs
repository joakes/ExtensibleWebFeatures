namespace DemoSite.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Optimization;
    using WebFeatures.Features;

    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/Content/less")
                .Include("~/Content/start.less")
                .Include(typeof(BaseWebFeature), "~/Content/", "webfeatures.less"));
        }
    }

    public static class BundleExtensions
    {
        public static Bundle Include(this Bundle bundle, Type type, string directoryVirtualPath, string importsLessFile)
        {
            var lessFileNames = CopyLessResourcesToServer(type, directoryVirtualPath);
            AppendLessFilesToStartScript(directoryVirtualPath, importsLessFile, lessFileNames);
            return bundle;
        }

        private static IEnumerable<string> CopyLessResourcesToServer(Type type, string directoryVirtualPath)
        {
            var assembly = type.Assembly;
            var resourceNames = assembly.GetManifestResourceNames().Where(res => res.EndsWith(".less"));

            var dir = BundleTable.MapPathMethod(directoryVirtualPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var lessFileNames = resourceNames as string[] ?? resourceNames.ToArray();
            foreach (var resourceName in lessFileNames)
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

            return lessFileNames;
        }

        private static void AppendLessFilesToStartScript(string directoryVirtualPath, string importsLessFile, IEnumerable<string> lessFileNames)
        {
            var virtualPath = Path.Combine(directoryVirtualPath, importsLessFile);
            var serverPath = BundleTable.MapPathMethod(virtualPath);

            using (var writer = File.AppendText(serverPath))
            {
                foreach (var lessFile in lessFileNames)
                {
                    writer.WriteLine("@import \"{0}\";", lessFile);
                }
            }
        }
    }
}