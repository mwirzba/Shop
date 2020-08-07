using FluentValidation;
using Shop.Dtos;

namespace Shop.Models
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(20);
                
            RuleFor(product => product.CategoryId)
                .NotEqual(0);

            RuleFor(product => product.Price)
                .GreaterThan(1)
                .LessThan(100000);
        }
    }
}
