using System;
using System.Linq;
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

        public override int Order
        {
            get
            {
                return -1000;
            }
        }

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

            var attribute = output.Attributes["name"];
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

            TryAddAttribute(output.Attributes, "data-val-required", "required", x => null);
            TryAddAttribute(output.Attributes, "data-val-minlength-min", "ng-minlength", x => x.Value);
            TryAddAttribute(output.Attributes, "data-val-maxlength-max", "ng-maxlength", x => x.Value);

            if (context.Items.ContainsKey(typeof(AngularFormTagHelper)))
            {
                ((AngularFormContext)context.Items[typeof(AngularFormTagHelper)]).InputAttributeLists.Add(For.Name, new TagHelperAttributeList(output.Attributes.ToList()));
            }
        }

        private void TryAddAttribute(TagHelperAttributeList attributes,
            string inputAttributeName, string angularAttributeName,
            Func<TagHelperAttribute, object> GetValue)
        {
            var attribute = attributes[inputAttributeName];
            if (attribute != null)
            {
                attributes.Add(angularAttributeName, GetValue(attribute));
            }
        }
    }
}
