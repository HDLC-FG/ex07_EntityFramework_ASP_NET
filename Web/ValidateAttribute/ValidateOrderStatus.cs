using System.ComponentModel.DataAnnotations;
using static ApplicationCore.Enums;

namespace Web.ValidateAttribute
{
    public class ValidateOrderStatus : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            var isParsed = Enum.TryParse(typeof(OrderStatus), value.ToString(), true, out object? result);

            if (!isParsed || isParsed && !Enum.IsDefined(typeof(OrderStatus), result!))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
