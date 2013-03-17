namespace DemoSite.Infrastructure
{
    using System;
    using System.Web;
    using System.IO;
    using System.Reflection;
    using System.Web.Hosting;

    class AssemblyResourceVirtualFile : VirtualFile
    {
        readonly string _path;

        public AssemblyResourceVirtualFile(string virtualPath) : base(virtualPath)
        {
            _path = VirtualPathUtility.ToAppRelative(virtualPath);
        }

        public override Stream Open()
        {
            var parts = _path.Split('/');
            var assemblyName = parts[2];
            var resourceName = parts[3];
            assemblyName = Path.Combine(HttpRuntime.BinDirectory, assemblyName);
            var assembly = Assembly.LoadFile(assemblyName);
            
            if (assembly == null) 
            {
                throw new Exception("Failed to load " + assemblyName);
            }
            
            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                throw new Exception("Failed to load " + resourceName);
            }

            return resourceStream;
        }
    }
}

//string path;
//    public AssemblyResourceVirtualFile(string virtualPath) : base(virtualPath)
//    {
//        path = VirtualPathUtility.ToAppRelative(virtualPath); 
//    }
//    public override System.IO.Stream Open()
//    {
//        string[] parts = path.Split('/');
//        string assemblyName = parts[2]; 
//        string resourceName = parts[3];
//        assemblyName = Path.Combine(HttpRuntime.BinDirectory, 
//                                    assemblyName);
//        System.Reflection.Assembly assembly = 
//           System.Reflection.Assembly.LoadFile(assemblyName);
//        if (assembly != null)
//        {
//            return assembly.GetManifestResourceStream(resourceName);
//        }
//        return null;
//    }