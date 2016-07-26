using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using PhantomNet.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcViewsServiceCollectionExtensions
    {
        public static IServiceCollection AddMvcViews(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options => {
                options.FileProviders.Add(new EmbeddedFileProvider(MvcViews.Assembly, MvcViews.Namespace));
            });

            return services;
        }
    }
}
