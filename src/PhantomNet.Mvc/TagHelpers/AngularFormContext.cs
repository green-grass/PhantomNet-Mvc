namespace PhantomNet.Mvc.TagHelpers
{
    public class AngularFormContext
    {
        public string Name { get; set; }

        public string ValidationCondition { get; set; }

        public string ValidationSuccessCssClass { get; set; }

        public string ValidationErrorCssClass { get; set; }

        public bool BootstrapStyle { get; set; }
    }
}
