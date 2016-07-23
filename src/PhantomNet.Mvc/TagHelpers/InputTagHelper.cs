using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PhantomNet.Mvc.TagHelpers
{
    [HtmlTargetElement(Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class InputTagHelper : Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper
    {
        private const string ForAttributeName = "pn-ng-for";

        public InputTagHelper(IHtmlGenerator generator) : base(generator) { }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression NgFor
        {
            get { return For; }
            set { For = value; }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            var maxlength = output.Attributes["data-val-maxlength-max"];
            if (maxlength != null)
            {
                output.Attributes.Add("ng-maxlength", maxlength.Value);
            }
        }
    }
}
