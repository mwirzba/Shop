using FluentValidation;
using Shop.Data.Repositories;
using Shop.Validation.CustomValidators;
using System.Collections.Generic;

namespace Shop.Validation
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, ICollection<TElement>> CartLinesMustHaveProductId<T, TElement>(this IRuleBuilder<T, ICollection<TElement>> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new ProductIdNotEmptyValidator());
        }

        public static IRuleBuilderOptions<T,TElement> ProductWithIdExists<T, TElement>(this IRuleBuilder<T,TElement> ruleBuilder,IUnitOfWork unitOfWork)
        {
            return ruleBuilder.SetValidator(new ProductWithIdExistsValidator(unitOfWork));
        }
        public static IRuleBuilderOptions<T, TElement> CartLineHasUniqueProductId<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new ProductIdUniqueValidator());
        }

        public static IRuleBuilderOptions<T, TElement> OrderStatusWithIdExists<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder, IUnitOfWork unitOfWork)
        {
            return ruleBuilder.SetValidator(new OrderStatusWithIdExistsValidator(unitOfWork));
        }
    }
}
