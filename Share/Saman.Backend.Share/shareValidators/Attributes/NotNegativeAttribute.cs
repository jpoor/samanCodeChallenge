using System.ComponentModel.DataAnnotations;
using Saman.Backend.Share.shareValidators.Rsx;

namespace Saman.Backend.Share.shareValidators.Attributes
{
    public class NotNegativeAttribute : ValidationAttribute
    {
        private readonly string? _fieldName;

        public NotNegativeAttribute(string? fieldName = null)
           => _fieldName = fieldName;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string fieldName = _fieldName ?? validationContext.DisplayName;
            string message = ErrorMessage ?? string.Format(Validators_Rsx.Validator_NotNegative, fieldName);

            if (value == null)
                return ValidationResult.Success;

            return value switch
            {
                int i when i >= 0 => ValidationResult.Success,
                long l when l >= 0 => ValidationResult.Success,
                short s when s >= 0 => ValidationResult.Success,
                decimal d when d >= 0 => ValidationResult.Success,
                double db when db >= 0 => ValidationResult.Success,
                float f when f >= 0 => ValidationResult.Success,

                _ => new ValidationResult(message)
            };
        }
    }
}