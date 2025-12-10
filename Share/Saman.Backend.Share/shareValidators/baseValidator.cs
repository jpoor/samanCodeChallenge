using System.ComponentModel.DataAnnotations;
using System.Text;
using Saman.Backend.Share.shareExceptions;

namespace Saman.Backend.Share.shareValidators
{
    public static class baseValidator
    {
        public static bool Validate<T>(T entity, out List<ValidationResult> results)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            var validationContext = new ValidationContext(entity);

            results = new List<ValidationResult>();

            return Validator.TryValidateObject(entity, validationContext, results, true);
        }

        public static bool ValidateAndThrowIfInvalid<T>(T entity)
        {
            // Validate
            List<ValidationResult> validationResults;
            bool isValid = Validate(entity, out validationResults);

            // Generate messages
            StringBuilder stringBuilder = new StringBuilder();
            if (!isValid)
                foreach (var result in validationResults)
                    stringBuilder.AppendLine(result.ErrorMessage);

            // Return result or Throw If Invalid
            return isValid
                 ? true
                 : throw new Exception_BadRequest(stringBuilder.ToString());
        }
    }
}
