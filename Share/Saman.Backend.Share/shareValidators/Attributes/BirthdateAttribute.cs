using System.ComponentModel.DataAnnotations;
using Saman.Backend.Share.shareValidators.Rsx;

namespace Saman.Backend.Share.shareValidators.Attributes
{
    public class BirthdateAttribute : ValidationAttribute
    {
        private readonly string? _fieldName;

        public BirthdateAttribute(string? fieldName = "Date of birth")
           => _fieldName = fieldName;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string fieldName = _fieldName ?? validationContext.DisplayName;
            string message = ErrorMessage ?? string.Format(Validators_Rsx.Validator_Birthdate, fieldName);

            if (value == null)
                return ValidationResult.Success;

            if (!(value is DateTime valueAsDateTime))
                return new ValidationResult(message);

            if (valueAsDateTime > DateTime.UtcNow.AddYears(-3))
                return new ValidationResult(message);

            return ValidationResult.Success;
        }
    }
}