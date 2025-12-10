using System.ComponentModel.DataAnnotations;
using Saman.Backend.Share.shareValidators.Rsx;

namespace Saman.Backend.Share.shareValidators.Attributes
{
    public class DateBeforeNowAttribute : ValidationAttribute
    {
        private readonly string? _fieldName;

        public DateBeforeNowAttribute(string? fieldName = null)
           => _fieldName = fieldName;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string fieldName = _fieldName ?? validationContext.DisplayName;
            string message = ErrorMessage ?? string.Format(Validators_Rsx.Validator_DateBeforeNow, fieldName);

            if (value == null)
                return ValidationResult.Success;

            if (!(value is DateTime valueAsDateTime))
                return new ValidationResult(message);

            if (valueAsDateTime > DateTime.UtcNow.AddSeconds(5))
                return new ValidationResult(message);

            return ValidationResult.Success;
        }
    }
}