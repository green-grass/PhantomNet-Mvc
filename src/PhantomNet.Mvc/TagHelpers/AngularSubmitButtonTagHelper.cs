﻿using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PhantomNet.Mvc.TagHelpers
{
    [HtmlTargetElement("button", Attributes = SubmitAttributeName)]
    [HtmlTargetElement("input", Attributes = SubmitAttributeName)]
    [HtmlTargetElement("a", Attributes = SubmitAttributeName)]
    public class AngularSubmitButtonTagHelper : TagHelper
    {
        private const string SubmitAttributeName = "pn-ng-submit";

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
                throw new InvalidOperationException(Resources.AngularSubmitButtonTagHelper_FormNameOrAngularFormRequired);
            }

            output.Attributes.Add("ng-disabled", $"{formName}.$invalid");
        }
    }
}
