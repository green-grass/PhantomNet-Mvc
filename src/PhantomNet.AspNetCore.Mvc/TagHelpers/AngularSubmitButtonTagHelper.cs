using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PhantomNet.AspNetCore.Mvc.Properties;

namespace PhantomNet.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("button", Attributes = SubmitAttributeName)]
    [HtmlTargetElement("input", Attributes = SubmitAttributeName)]
    [HtmlTargetElement("a", Attributes = SubmitAttributeName)]
    public class AngularSubmitButtonTagHelper : TagHelper
    {
        private const string SubmitAttributeName = "pn-ng-submit";
        private const string FormNameAttributeName = "form-name";

        public string FormName { get; set; }

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

            if (!context.Items.ContainsKey(typeof(AngularFormTagHelper)))
            {
                return;
            }

            var formContext = (AngularFormContext)context.Items[typeof(AngularFormTagHelper)];
            var formName = FormName ?? formContext.Name;
            if (string.IsNullOrWhiteSpace(formName))
            {
                throw new InvalidOperationException(string.Format(Strings.FormNameOrAngularFormRequired,
                    SubmitAttributeName, FormNameAttributeName, AngularFormTagHelper.NameAttributeName));
            }

            output.Attributes.Add("ng-disabled", $"{formName}.$invalid");
        }
    }
}
