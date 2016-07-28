using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PhantomNet.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("span", Attributes = ValidationForAttributeName)]
    public class AngularValidationMessageTagHelper : TagHelper
    {
        private const string ValidationForAttributeName = "pn-ng-validation-for";
        private const string FormNameAttributeName = "form-name";

        private const string DefaultCondition = ".$touched";

        [HtmlAttributeName(ValidationForAttributeName)]
        public ModelExpression For { get; set; }

        public string FormName { get; set; }

        public string Condition { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (!context.Items.ContainsKey(typeof(AngularFormTagHelper)))
            {
                return;
            }

            var formContext = (AngularFormContext)context.Items[typeof(AngularFormTagHelper)];
            if (!formContext.InputAttributeLists.ContainsKey(For.Name))
            {
                return;
            }

            var inputAttributes = formContext.InputAttributeLists[For.Name];

            var formName = FormName ?? formContext.Name;
            if (string.IsNullOrWhiteSpace(formName))
            {
                throw new InvalidOperationException(Resources.FormatFormNameOrAngularFormRequired(
                    ValidationForAttributeName, FormNameAttributeName, AngularFormTagHelper.NameAttributeName));
            }

            var fieldName = For.Name.ToCamelCase();
            var fieldPath = $"{formName}.{fieldName}";

            var condition = Condition ?? formContext.ValidationCondition ?? DefaultCondition;
            if (condition.StartsWith("."))
            {
                condition = $"{fieldPath}{condition}";
            }
            else if (condition.StartsWith("!."))
            {
                condition = $"!{fieldPath}{condition.Substring(1)}";
            }

            output.Attributes.Add("ng-messages", $"{fieldPath}.$error");
            output.Attributes.Add("ng-if", condition);

            var childContent = output.Content.IsModified ? output.Content.GetContent() :
                (await output.GetChildContentAsync()).GetContent();

            var tagBuilder = new TagBuilder("span");
            tagBuilder.InnerHtml.AppendHtmlLine(childContent);

            TryAddAttribute(inputAttributes, tagBuilder.InnerHtml, "data-val-required", "required");
            TryAddAttribute(inputAttributes, tagBuilder.InnerHtml, "data-val-minlength", "minlength");
            TryAddAttribute(inputAttributes, tagBuilder.InnerHtml, "data-val-maxlength", "maxlength");
            TryAddAttribute(inputAttributes, tagBuilder.InnerHtml, "data-val-email", "email");

            output.Content.AppendHtml(tagBuilder.InnerHtml);
        }

        private void TryAddAttribute(TagHelperAttributeList attributes, IHtmlContentBuilder html,
            string inputAttributeName, string angularMessageKey)
        {
            var attribute = attributes[inputAttributeName];
            if (attribute != null)
            {
                html.AppendHtmlLine($"<span ng-message=\"{angularMessageKey}\">{attribute.Value}</span>");
            }
        }
    }
}
