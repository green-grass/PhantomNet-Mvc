using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PhantomNet.Mvc.TagHelpers
{
    [HtmlTargetElement(Attributes = GroupForAttributeName + "," + SuccessCssClassAttributeName + "," + ErrorCssClassAttributeName)]
    [HtmlTargetElement(Attributes = GroupForAttributeName + "," + BootstrapStyleAttributeName)]
    [HtmlTargetElement(Attributes = GroupForAttributeName)]
    public class AngularInputGroupTagHelper : TagHelper
    {
        private const string GroupForAttributeName = "pn-ng-group-for";
        private const string BootstrapStyleAttributeName = "bootstrap-style";
        private const string SuccessCssClassAttributeName = "success-css-class";
        private const string ErrorCssClassAttributeName = "error-css-class";

        private const string DefaultCondition = ".$touched";
        private const string BootstrapSuccessCssClass = "has-success";
        private const string BootstrapErrorCssClass = "has-error";

        [HtmlAttributeName(GroupForAttributeName)]
        public virtual ModelExpression GroupFor { get; set; }

        public string FormName { get; set; }

        public string Condition { get; set; }

        public string SuccessCssClass { get; set; }

        public string ErrorCssClass { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            output.Attributes.RemoveAll(BootstrapStyleAttributeName);
            output.Attributes.RemoveAll(SuccessCssClassAttributeName);
            output.Attributes.RemoveAll(ErrorCssClassAttributeName);

            var formContext = context.Items.Keys.Contains(typeof(AngularFormTagHelper)) ? (AngularFormContext)context.Items[typeof(AngularFormTagHelper)] : null;

            var formName = FormName ?? formContext?.Name;
            if (string.IsNullOrWhiteSpace(formName))
            {
                throw new InvalidOperationException(Resources.FormNameOrAngularFormRequired);
            }

            var fieldName = GroupFor.Metadata.GetDisplayName().ToCamelCase();
            var fieldPath = $"{formName}.{fieldName}";

            var condition = Condition ?? formContext?.ValidationCondition ?? DefaultCondition;
            if (condition.StartsWith("."))
            {
                condition = $"{fieldPath}{condition}";
            }
            else if (condition.StartsWith("!."))
            {
                condition = $"!{fieldPath}{condition.Substring(1)}";
            }

            var successCssClass = SuccessCssClass ??
                                  (context.AllAttributes.ContainsName(BootstrapStyleAttributeName) ? BootstrapSuccessCssClass : null) ??
                                  formContext?.ValidationSuccessCssClass ??
                                  (formContext?.BootstrapStyle == true ? BootstrapSuccessCssClass : null);

            var errorCssClass = ErrorCssClass ??
                                (context.AllAttributes.ContainsName(BootstrapStyleAttributeName) ? BootstrapErrorCssClass : null) ??
                                formContext?.ValidationErrorCssClass ??
                                (formContext?.BootstrapStyle == true ? BootstrapErrorCssClass : null);


            if (string.IsNullOrWhiteSpace(successCssClass))
            {
                throw new InvalidOperationException();
            }

            if (string.IsNullOrWhiteSpace(errorCssClass))
            {
                throw new InvalidOperationException();
            }

            var ngClass = $"{{ '{successCssClass}': {condition} && {fieldPath}.$valid, '{errorCssClass}': {condition} && {fieldPath}.$invalid }}";
            output.Attributes.Add("ng-class", ngClass);
        }
    }
}
