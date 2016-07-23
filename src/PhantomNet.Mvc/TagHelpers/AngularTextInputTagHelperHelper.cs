using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PhantomNet.Mvc.TagHelpers
{
    internal static class AngularTextInputTagHelperHelper
    {
        public static void Process(
            TagHelperContext context, TagHelperOutput output,
            ModelExpression angularFor, string model, string defaultModelValue, string placeholder)
        {
            TagHelperAttribute attribute;

            attribute = output.Attributes["name"];
            if (attribute != null)
            {
                var nameValue = attribute.Value.ToString().ToCamelCase();
                output.Attributes.SetAttribute("name", nameValue);

                string ngModelValue;
                if (model == null)
                {
                    ngModelValue = nameValue;
                }
                else if (string.IsNullOrWhiteSpace(model))
                {
                    ngModelValue = $"{defaultModelValue}.{nameValue}";
                }
                else
                {
                    ngModelValue = $"{model}.{nameValue}";
                }
                output.Attributes.Add("ng-model", ngModelValue);
            }

            string placeholderValue;
            if (placeholder == null)
            {
                placeholderValue = null;
            }
            else if (string.IsNullOrWhiteSpace(placeholder))
            {
                placeholderValue = angularFor.Metadata.GetDisplayName();
            }
            else if (placeholder.Contains("{0}"))
            {
                placeholderValue = placeholder.Replace("{0}", angularFor.Metadata.GetDisplayName());
            }
            else
            {
                placeholderValue = placeholder;
            }
            output.Attributes.SetAttribute("placeholder", placeholderValue);

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
