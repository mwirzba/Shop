using FluentValidation;
using Shop.Data.Repositories;
using Shop.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Models
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {
        private IUnitOfWork _unitOfWork;
        public CategoryValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(category => category.Name)
              .NotEmpty()
              .MinimumLength(2)
              .MaximumLength(20)
              .Must(IsNameUnique)
              .WithMessage("Invalid category name");
        }

        public bool IsNameUnique(CategoryDto category, string newValue)
        {
            var categoryInDb = _unitOfWork.Categories.SingleOrDefault(c => c.Name == category.Name);

            if (categoryInDb == null)
                return true;
            return false;
        }
    }
}