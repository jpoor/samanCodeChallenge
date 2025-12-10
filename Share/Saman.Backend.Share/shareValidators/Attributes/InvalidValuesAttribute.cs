using System.ComponentModel.DataAnnotations;
using Saman.Backend.Share.shareValidators.Rsx;

namespace Saman.Backend.Share.shareValidators.Attributes
{
    public class InvalidValuesAttribute : ValidationAttribute
    {
        private readonly object[] _invalidValues;
        private readonly string? _fieldName;

        public InvalidValuesAttribute(string? fieldName = null, params object[] invalidValues)
        {
            _fieldName = fieldName;
            _invalidValues = invalidValues;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string fieldName = _fieldName ?? validationContext.DisplayName;
            string invalidValuesList = string.Join(", ", _invalidValues);
            string message = ErrorMessage ?? string.Format(Validators_Rsx.Validator_InvalidValues, fieldName, invalidValuesList);

            if (value == null)
                return ValidationResult.Success;

            foreach (var invalidValue in _invalidValues)
                if (value.Equals(invalidValue))
                    return new ValidationResult(message);

            return ValidationResult.Success;
        }
    }
}
