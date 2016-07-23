using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PhantomNet.Mvc.TagHelpers
{
    [HtmlTargetElement("textarea", Attributes = ForAttributeName)]
    public class AngularTextAreaTagHelper : TextAreaTagHelper
    {
        private const string ForAttributeName = "pn-ng-for";
        private const string DefaultModelValue = "model";

        public AngularTextAreaTagHelper(IHtmlGenerator generator) : base(generator) { }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression AngularFor
        {
            get { return For; }
            set { For = value; }
        }

        public string Model { get; set; }

        public string Placeholder { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            AngularTextInputTagHelperHelper.Process(context, output,
                AngularFor, Model, DefaultModelValue, Placeholder);
        }
    }
}
