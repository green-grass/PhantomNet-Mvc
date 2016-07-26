using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace PhantomNet.AspNetCore.Mvc
{
    public class MvcHelper
    {
        private readonly RouteData _routeData;

        public MvcHelper(IHttpContextAccessor httpContextAccessor)
        {
            _routeData = httpContextAccessor.HttpContext.GetRouteData();
        }

        public bool Compozing => _routeData.Values[RouteBuilderExtensions.CompozrRouteValueName] != null;

        public string CurrentActionCompozrHeadViewComponentName => $"{CurrentActionCompozrViewComponentName}Head";

        public string CurrentActionCompozrBodyViewComponentName => $"{CurrentActionCompozrViewComponentName}Body";

        public string CurrentActionCompozrScriptsViewComponentName => $"{CurrentActionCompozrViewComponentName}Scripts";

        public string BuildTitle<TModel>(ViewDataDictionary<TModel> viewData, string siteName)
            => viewData["Title"] == null || string.IsNullOrWhiteSpace((string)viewData["Title"]) || (string)viewData["Title"] == siteName ? siteName : $"{viewData["Title"]} - {siteName}";

        public bool IsMenuItemActive<TModel>(ViewDataDictionary<TModel> viewData, string menuItem)
            => (string)viewData["ActiveMenuItem"] == menuItem;

        public string ActiveMenuItem<TModel>(ViewDataDictionary<TModel> viewData, string menuItem)
            => IsMenuItemActive(viewData, menuItem) ? "active" : null;

        public bool IsSubMenuItemActive<TModel>(ViewDataDictionary<TModel> viewData, string subMenuItem)
            => (string)viewData["ActiveSubMenuItem"] == subMenuItem;

        public string ActiveSubMenuItem<TModel>(ViewDataDictionary<TModel> viewData, string subMenuItem)
            => IsSubMenuItemActive(viewData, subMenuItem) ? "active" : null;

        private string CurrentActionCompozrViewComponentName
            => $"{_routeData.Values["controller"]}{_routeData.Values["action"]}Compozr";
    }
}
