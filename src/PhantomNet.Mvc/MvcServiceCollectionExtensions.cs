using Microsoft.AspNetCore.Mvc.Razor;
using PhantomNet.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalizedMvc(this IServiceCollection services, string defaultCulture, params string[] supportedCultures)
        {
            services.AddLocalization(defaultCulture, supportedCultures);

            services.AddMvc()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization();

            services.AddScoped<MvcHelper>();

            return services;
        }
    }
}
