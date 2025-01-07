using System.ComponentModel.DataAnnotations;

namespace Web.ValidateAttribute
{
    public class ValidateTotalAmount : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }
            var amount = (double)value!;
            if (amount <= 0)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
