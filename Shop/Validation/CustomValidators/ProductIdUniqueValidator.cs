using FluentValidation.Validators;
using Shop.Dtos;
using System.Linq;


namespace Shop.Validation.CustomValidators
{
    public class ProductIdUniqueValidator: PropertyValidator
    {
        public ProductIdUniqueValidator(): base("Product with Id: {ProductId} is repeated {numberOfProd} times!") {}
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var cartLine = context.PropertyValue as CartLineRequest;
            var order = context.ParentContext.InstanceToValidate as OrderRequest;
            var cartLines = order.CartLines;

            var numberOfProductsWithId = cartLines.Where(cl => cl.ProductId == cartLine.ProductId).Count();

            if(numberOfProductsWithId > 1)
            {
                context.MessageFormatter.AppendArgument("ProductId", cartLine.ProductId);
                context.MessageFormatter.AppendArgument("numberOfProd", numberOfProductsWithId);
                return false;
            }
            return true;
        }
    }
}
