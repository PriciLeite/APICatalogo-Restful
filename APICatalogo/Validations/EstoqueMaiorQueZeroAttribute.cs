using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validations
{
    public class EstoqueMaiorQueZeroAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int estoque = Convert.ToInt32(value);
                if (estoque <= 0)
                {
                    return new ValidationResult(ErrorMessage ?? "O estoque deve ser maior que zero.");
                }
            }
            
            return ValidationResult.Success;
        }

    }
}
