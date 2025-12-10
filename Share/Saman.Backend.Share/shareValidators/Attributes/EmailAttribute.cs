using System.ComponentModel.DataAnnotations;
using Saman.Backend.Share.shareValidators.Rsx;

namespace Saman.Backend.Share.shareValidators.Attributes
{
    public class EmailAttribute : ValidationAttribute
    {
        private readonly string? _fieldName;

        public EmailAttribute(string? fieldName = "Email address")
           => _fieldName = fieldName;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string fieldName = _fieldName ?? validationContext.DisplayName;
            string message = ErrorMessage ?? string.Format(Validators_Rsx.Validator_Email, fieldName);

            var emailAttr = new EmailAddressAttribute();

            return emailAttr.IsValid(value)
                 ? ValidationResult.Success
                 : new ValidationResult(message);
        }
    }
}