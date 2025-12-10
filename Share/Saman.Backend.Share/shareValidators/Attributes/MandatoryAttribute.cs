using System.ComponentModel.DataAnnotations;
using Saman.Backend.Share.shareValidators.Rsx;

namespace Saman.Backend.Share.shareValidators.Attributes
{
    public class MandatoryAttribute : ValidationAttribute
    {
        private readonly string? _fieldName;

        public MandatoryAttribute(string? fieldName = null)
           => _fieldName = fieldName;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string fieldName = _fieldName ?? validationContext.DisplayName;
            string message = ErrorMessage ?? string.Format(Validators_Rsx.Validator_Mandatory, fieldName);

            if (value == null)
                return new ValidationResult(message);

            if (value is string valueAsString && string.IsNullOrWhiteSpace(valueAsString))
                return new ValidationResult(message);

            return ValidationResult.Success;
        }
    }
}