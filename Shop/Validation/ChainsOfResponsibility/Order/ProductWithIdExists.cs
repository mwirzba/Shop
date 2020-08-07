using Shop.Data.Repositories;
using Shop.Dtos;
using Shop.Models;
using System.Collections.Generic;

namespace Shop.Validation.ChainsOfResponsibility
{
    public class ProductWithIdExistsValidator: ValidatorBase<OrderRequest>
    {
        private IUnitOfWork _unitOfWork;
        public ProductWithIdExistsValidator(IUnitOfWork unitOfWork): base()
        {
            _unitOfWork = unitOfWork;
        }
        public override Dictionary<string, string> HandleValidation(OrderRequest orderRequest)
        {
            Product productInDb;
            foreach (var cartLine in orderRequest.CartLines)
            {
                productInDb = _unitOfWork.Products.Get(cartLine.ProductId);
                if(productInDb == null)
                {
                    ErrorsResult.Add("ProductId ", $"Product with Id: {cartLine.ProductId} doesn't  exists");
                }
            }

            if (Successor != null)
                return Successor.HandleValidation(orderRequest);

            return ErrorsResult;
        }
    }
}
