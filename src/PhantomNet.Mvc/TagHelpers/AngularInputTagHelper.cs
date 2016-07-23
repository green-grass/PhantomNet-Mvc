using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PhantomNet.Mvc.TagHelpers
{
    [HtmlTargetElement("input", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class AngularInputTagHelper : InputTagHelper
    {
        private const string ForAttributeName = "pn-ng-for";
        private const string DefaultModelValue = "model";

        public AngularInputTagHelper(IHtmlGenerator generator) : base(generator) { }

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
