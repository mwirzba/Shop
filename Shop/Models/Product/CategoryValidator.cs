using FluentValidation;
using Shop.Data.Repositories;
using Shop.Models.ProductDtos;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Models
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {
        private List<Category> categories;
        public CategoryValidator(IUnitOfWork unitOfWork)
        {
            categories = (List<Category>)unitOfWork.Categories.GetAllAsync().Result;

            RuleFor(category => category.Name)
              .NotEmpty()
              .MinimumLength(2)
              .MaximumLength(20)
              .Must(IsNameUnique)
              .WithMessage("Invalid category name");
              
        }

        public bool IsNameUnique(CategoryDto category, string newValue)
        {
            var categoryInDb = categories.FirstOrDefault(c => c.Name == newValue);

            if (category == null)
                return true;
            return false;
            
        }
    }
}
