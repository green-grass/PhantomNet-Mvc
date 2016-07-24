using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PhantomNet.Mvc.TagHelpers
{
    [HtmlTargetElement("span", Attributes = ValidationForAttributeName)]
    public class AngularValidationMessageTagHelper : TagHelper
    {
        private const string ValidationForAttributeName = "pn-ng-validation-for";

        public AngularValidationMessageTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        protected IHtmlGenerator Generator { get; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ValidationForAttributeName)]
        public ModelExpression For { get; set; }

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

            var inputTagHelper = new InputTagHelper(Generator) {
                ViewContext = ViewContext,
                For = For
            };
            var inputTagHelperContext = new TagHelperContext(new TagHelperAttributeList(),
                                                             new Dictionary<object, object>(),
                                                             Guid.NewGuid().ToString());
            var inputTagHelperOutput = new TagHelperOutput("input",
                                                            new TagHelperAttributeList(),
                                                            (useCachedResult, encoder) => Task.FromResult(new DefaultTagHelperContent() as TagHelperContent)) {
                TagMode = TagMode.SelfClosing
            };
            inputTagHelper.Process(inputTagHelperContext, inputTagHelperOutput);

            var formName = "";
            var fieldName = "";
            var fieldPath = $"{formName}.{fieldName}";

            var condition = "";
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

            TagHelperAttribute attribute;
            attribute = inputTagHelperOutput.Attributes["data-val-required"];
            if (attribute != null)
            {
                tagBuilder.InnerHtml.AppendHtmlLine($"<span ng-message=\"required\">{attribute.Value}</span>");
            }

            output.Content.AppendHtml(tagBuilder.InnerHtml);
        }
    }
}
