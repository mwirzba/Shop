using FluentValidation.Validators;
using Shop.Data.Repositories;
using Shop.Models;


namespace Shop.Validation.CustomValidators
{
    public class OrderStatusWithIdExistsValidator: PropertyValidator
    {
        private IUnitOfWork _unitOfWork;
        public OrderStatusWithIdExistsValidator(IUnitOfWork unitOfWork) : base("OrderStatus with Id: {statusId} doesn't exists")
        {
            _unitOfWork = unitOfWork;
        }
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var orderStatusId = (int)context.PropertyValue;

            var orderInDb = _unitOfWork.OrderStatuses.Get(orderStatusId);
            if (orderInDb == null)
            {
                context.MessageFormatter.AppendArgument("statusId", orderStatusId);
                return false;
            }

            return true;
        }
    }
}
