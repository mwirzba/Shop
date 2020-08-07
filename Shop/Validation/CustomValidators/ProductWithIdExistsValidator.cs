using FluentValidation.Validators;
using Shop.Data.Repositories;
using Shop.Dtos;

namespace Shop.Validation.CustomValidators
{
    public class ProductWithIdExistsValidator: PropertyValidator
    {
        private IUnitOfWork _unitOfWork;
     
        public ProductWithIdExistsValidator(IUnitOfWork unitOfWork) : base("Product with Id: {ProductId} doesn't exists")
        {
            _unitOfWork = unitOfWork;
        }
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var cartLine = context.PropertyValue as CartLineRequest;
           
            var productInDb = _unitOfWork.Products.Get(cartLine.ProductId);
            if (productInDb == null)
            {
                context.MessageFormatter.AppendArgument("ProductId", cartLine.ProductId);
                return false;
            }
            
            return true;
        }
    }
}
