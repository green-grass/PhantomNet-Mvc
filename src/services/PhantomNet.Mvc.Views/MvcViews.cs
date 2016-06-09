using System.Reflection;

namespace PhantomNet.Mvc
{
    public static class MvcViews
    {
        public const string CompozrHeadLayout = "Components/_Layouts/CompozrHead";
        public const string CompozrBodyLayout = "Components/_Layouts/CompozrBody";
        public const string CompozrScriptsLayout = "Components/_Layouts/CompozrScripts";

        public const string CompozrHeadPartial = "_CompozrHeadPartial";
        public const string CompozrBodyPartial = "_CompozrBodyPartial";
        public const string CompozrScriptsPartial = "_CompozrScriptsPartial";

        public static Assembly Assembly => typeof(MvcViews).GetTypeInfo().Assembly;

        public static string Namespace => typeof(MvcViews).GetTypeInfo().Namespace;
    }
}
