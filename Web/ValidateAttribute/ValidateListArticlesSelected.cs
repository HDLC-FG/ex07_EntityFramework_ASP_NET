using System.ComponentModel.DataAnnotations;
using Web.ViewModels;

namespace Web.ValidateAttribute
{
    public class ValidateListArticlesSelected : ValidationAttribute
    {
        private readonly int minCount;

        public ValidateListArticlesSelected(int minCount)
        {
            this.minCount = minCount;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var listArticleSelected = value as List<ArticleSelectedViewModel>;

            if (listArticleSelected == null || listArticleSelected.Count < minCount)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
