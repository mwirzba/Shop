using FluentValidation;
using Shop.Dtos;

namespace Shop.Validation
{
    public class OrderStatusValidator :  AbstractValidator<OrderStatusDto>
    {
        public OrderStatusValidator()
        {
            RuleFor(os => os.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(20);
        }
    }
}
