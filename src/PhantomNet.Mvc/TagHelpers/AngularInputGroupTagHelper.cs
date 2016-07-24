using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PhantomNet.Mvc.TagHelpers
{
    [HtmlTargetElement(Attributes = GroupForAttributeName)]
    public class InheritAngularInputGroupTagHelper : AngularInputGroupTagHelper
    {
        private const string BootstrapStyleAttributeName = "bootstrap-style";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context.AllAttributes.ContainsName(BootstrapStyleAttributeName) ||
                (context.AllAttributes.ContainsName(SuccessCssClassAttributeName) &&
                 context.AllAttributes.ContainsName(ErrorCssClassAttributeName)))
            {
                return;
            }

            var formContext = context.Items.Keys.Contains(typeof(AngularFormTagHelper)) ? (AngularFormContext)context.Items[typeof(AngularFormTagHelper)] : null;

            if (!string.IsNullOrWhiteSpace(formContext.ValidationSuccessCssClass))
            {
                SuccessCssClass = SuccessCssClass ?? formContext.ValidationSuccessCssClass;
            }

            if (!string.IsNullOrWhiteSpace(formContext.ValidationErrorCssClass))
            {
                ErrorCssClass = ErrorCssClass ?? formContext.ValidationErrorCssClass;
            }

            if (formContext.BootstrapStyle)
            {
                SuccessCssClass = SuccessCssClass ?? "has-success";
                ErrorCssClass = ErrorCssClass ?? "has-error";
            }

            if (string.IsNullOrWhiteSpace(SuccessCssClass))
            {
                throw new InvalidOperationException();
            }

            if (string.IsNullOrWhiteSpace(ErrorCssClass))
            {
                throw new InvalidOperationException();
            }

            base.Process(context, output);
        }
    }

    [HtmlTargetElement(Attributes = GroupForAttributeName + "," + BootstrapStyleAttributeName)]
    public class BootstrapAngularInputGroupTagHelper : AngularInputGroupTagHelper
    {
        private const string BootstrapStyleAttributeName = "bootstrap-style";

        public BootstrapAngularInputGroupTagHelper()
        {
            SuccessCssClass = "has-success";
            ErrorCssClass = "has-error";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            output.Attributes.RemoveAll(BootstrapStyleAttributeName);
        }
    }

    [HtmlTargetElement(Attributes = GroupForAttributeName + "," + SuccessCssClassAttributeName + "," + ErrorCssClassAttributeName)]
    public class AngularInputGroupTagHelper : TagHelper
    {
        protected const string GroupForAttributeName = "pn-ng-group-for";
        protected const string SuccessCssClassAttributeName = "success-css-class";
        protected const string ErrorCssClassAttributeName = "error-css-class";
        private const string DefaultCondition = ".$touched";

        [HtmlAttributeName(GroupForAttributeName)]
        public virtual ModelExpression GroupFor { get; set; }

        public string FormName { get; set; }

        public string Condition { get; set; }

        public string SuccessCssClass { get; set; }

        public string ErrorCssClass { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll(SuccessCssClassAttributeName);
            output.Attributes.RemoveAll(ErrorCssClassAttributeName);

            var formContext = context.Items.Keys.Contains(typeof(AngularFormTagHelper)) ? (AngularFormContext)context.Items[typeof(AngularFormTagHelper)] : null;

            var formName = FormName;
            if (string.IsNullOrWhiteSpace(FormName))
            {
                formName = formContext?.Name;
                if (string.IsNullOrWhiteSpace(formName))
                {
                    throw new InvalidOperationException(Resources.FormNameOrAngularFormRequired);
                }
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

            var ngClass = $"{{ '{SuccessCssClass}': {condition} && {fieldPath}.$valid, '{ErrorCssClass}': {condition} && {fieldPath}.$invalid }}";
            output.Attributes.Add("ng-class", ngClass);
        }
    }
}
