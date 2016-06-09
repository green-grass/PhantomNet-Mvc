using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace PhantomNet.Mvc.FileProviders
{
    public abstract class PhysicalFileProviderBase
    {
        public PhysicalFileProviderBase(string defaultVirtualBasePath, IHostingEnvironment env, IOptions<PhysicalFileProviderOptions> optionsAccessor)
        {
            VirtualBasePath = optionsAccessor?.Value?.VirtualBasePath ?? defaultVirtualBasePath;
            PhysicalBasePath = env.WebRootPath;
        }

        protected string VirtualBasePath { get; set; }

        protected string PhysicalBasePath { get; set; }

        protected string GenerateTourVirtualPath(string key) => Path.Combine(VirtualBasePath, key);

        protected string GenerateTourPhysicalPath(string key) => Path.Combine(PhysicalBasePath, GenerateTourVirtualPath(key).TrimStart('~').TrimStart('/'));

        protected string FormatVirtualPath(string key, string physicalPath)
        {
            return Path.Combine(GenerateTourVirtualPath(key), Path.GetFileName(physicalPath)).Replace('\\', '/');
        }

        protected string FormatVirtualPath(string key, string relativePath, string physicalPath)
        {
            return Path.Combine(GenerateTourVirtualPath(key), relativePath, Path.GetFileName(physicalPath)).Replace('\\', '/');
        }
    }
}
