using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PhantomNet.Mvc.TagHelpers
{
    [HtmlTargetElement("form", Attributes = NameAttributeName + "," + ValidationSuccessCssClassAttributeName + "," + ValidationErrorCssClassAttributeName)]
    public class HasCssClassesAngularFormTagHelper : AngularFormTagHelper
    {
        private const string ValidationSuccessCssClassAttributeName = "validation-success-css-class";
        private const string ValidationErrorCssClassAttributeName = "validation-error-css-class";

        public string ValidationSuccessCssClass { get; set; }

        public string ValidationErrorCssClass { get; set; }

        public override int Order { get { return 0; } }

        public override void Init(TagHelperContext context)
        {
            var formContext = (AngularFormContext)context.Items[typeof(AngularFormTagHelper)];
            formContext.ValidationSuccessCssClass = ValidationSuccessCssClass;
            formContext.ValidationErrorCssClass = ValidationErrorCssClass;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll(ValidationSuccessCssClassAttributeName);
            output.Attributes.RemoveAll(ValidationErrorCssClassAttributeName);
        }
    }

    [HtmlTargetElement("form", Attributes = NameAttributeName + "," + BootstrapStyleAttributeName)]
    public class BootstrapAngularFormTagHelper : AngularFormTagHelper
    {
        private const string BootstrapStyleAttributeName = "bootstrap-style";

        public override int Order { get { return 0; } }

        public override void Init(TagHelperContext context)
        {
            ((AngularFormContext)context.Items[typeof(AngularFormTagHelper)]).BootstrapStyle = true;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll(BootstrapStyleAttributeName);
        }
    }

    [HtmlTargetElement("form", Attributes = NameAttributeName)]
    public class AngularFormTagHelper : TagHelper
    {
        protected const string NameAttributeName = "pn-ng-name";

        [HtmlAttributeName(NameAttributeName)]
        public string Name { get; set; }

        public string ValidationCondition { get; set; }

        public override int Order { get { return int.MinValue; } }

        public override void Init(TagHelperContext context)
        {
            context.Items.Add(typeof(AngularFormTagHelper), new AngularFormContext {
                Name = Name,
                ValidationCondition = ValidationCondition,
                BootstrapStyle = false
            });
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("name", Name);
        }
    }
}
