using FluentValidation;
using Shop.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Validation
{
    public class OrderValidator : AbstractValidator<OrderDto>
    {
        public OrderValidator()
        {
            RuleFor(order => order.CartLines)
              .NotEmpty()
             .WithMessage("Order can not be empty ");

            RuleFor(order => order.Name)
             .NotEmpty()
             .WithMessage("Name is required.");

            RuleFor(order => order.StatusId)
                .NotNull();
        }
    }
}
