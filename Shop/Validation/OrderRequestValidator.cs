using FluentValidation;
using Shop.Data.Repositories;
using Shop.Dtos;
using Shop.Validation;

namespace Shop.Models.Validation
{
    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        public OrderRequestValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(order => order.CartLines)
              .NotEmpty()
             .WithMessage("Order can not be empty ");

            RuleFor(order => order.Name)
             .NotEmpty()
             .WithMessage("Name is required.");

            RuleFor(order => order.StatusId)
                .OrderStatusWithIdExists(unitOfWork);


            RuleForEach(cl => cl.CartLines)
                .ChildRules(l =>
                {
                    l.RuleFor(i => i.ProductId)
                    .NotEqual(0)
                    .WithMessage("Product id can not be equal 0.");

                    l.RuleFor(i => i.Quantity)
                    .GreaterThan(0);
                });


            RuleForEach(cl => cl.CartLines)
                .ProductWithIdExists(unitOfWork);

            RuleForEach(cl => cl.CartLines)
                .CartLineHasUniqueProductId();
        }
    }
}
