using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PhantomNet.Mvc.TagHelpers
{
    [HtmlTargetElement("input", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("textarea", Attributes = ForAttributeName)]
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

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            TagHelperAttribute attribute;

            attribute = output.Attributes["name"];
            if (attribute != null)
            {
                var name = attribute.Value.ToString().ToCamelCase();
                output.Attributes.SetAttribute("name", name);

                string ngModel;
                if (Model == null)
                {
                    ngModel = name;
                }
                else if (string.IsNullOrWhiteSpace(Model))
                {
                    ngModel = $"{DefaultModelValue}.{name}";
                }
                else
                {
                    ngModel = $"{Model}.{name}";
                }
                output.Attributes.Add("ng-model", ngModel);
            }

            attribute = output.Attributes["data-val-required"];
            if (attribute != null)
            {
                output.Attributes.Add("required", null);
            }

            attribute = output.Attributes["data-val-minlength-min"];
            if (attribute != null)
            {
                output.Attributes.Add("ng-minlength", attribute.Value);
            }

            attribute = output.Attributes["data-val-maxlength-max"];
            if (attribute != null)
            {
                output.Attributes.Add("ng-maxlength", attribute.Value);
            }
        }
    }
}
