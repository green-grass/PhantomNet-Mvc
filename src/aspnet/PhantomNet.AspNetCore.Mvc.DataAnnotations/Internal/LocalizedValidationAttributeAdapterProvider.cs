using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace Microsoft.AspNetCore.Mvc.DataAnnotations.Internal
{
    public class LocalizedValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly ValidationAttributeAdapterProvider _internalProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            IAttributeAdapter adapter;

            var type = attribute.GetType();

            if (type == typeof(LocalizedRequiredAttribute))
            {
                adapter = new RequiredAttributeAdapter((RequiredAttribute)attribute, stringLocalizer);
            }
            else if (type == typeof(LocalizedMinLengthAttribute))
            {
                adapter = new MinLengthAttributeAdapter((MinLengthAttribute)attribute, stringLocalizer);
            }
            else if (type == typeof(LocalizedMaxLengthAttribute))
            {
                adapter = new MaxLengthAttributeAdapter((MaxLengthAttribute)attribute, stringLocalizer);
            }
            else if (type == typeof(LocalizedEmailAddressAttribute))
            {
                adapter = new DataTypeAttributeAdapter((DataTypeAttribute)attribute, "data-val-email", stringLocalizer);
            }
            else
            {
                adapter = _internalProvider.GetAttributeAdapter(attribute, stringLocalizer);
            }

            return adapter;
        }
    }
}
