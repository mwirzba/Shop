using FluentValidation.Validators;
using Shop.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Validation.CustomValidators
{
    public class ProductIdNotEmptyValidator : PropertyValidator
    {
        public ProductIdNotEmptyValidator() : base("ProductId in cartLine can not be 0") {}
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var cartLines = context.PropertyValue as ICollection<CartLineRequest>;
            var cartLinesWithNullOrder = cartLines.FirstOrDefault(o => o.ProductId == 0);
            if (cartLinesWithNullOrder != null)
            {
                context.MessageFormatter.AppendArgument("ProductId",0);
                return false;
            }
            return true;
        }
    }
}
