using FluentValidation;


namespace Shop.Models
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(u => u.UserName)
                .NotNull()
                .NotEmpty();

            RuleFor(u => u.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}
