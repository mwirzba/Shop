

using Shop.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Validation.ChainsOfResponsibility
{
    public class ProductIdRequiredValidator:  ValidatorBase<OrderRequest>
    {
        public override Dictionary<string, string> HandleValidation(OrderRequest orderRequest)
        {
            var cartLinesWithNullOrder = orderRequest.CartLines.FirstOrDefault(o =>  o.ProductId == 0);
            if(cartLinesWithNullOrder != null)
            {
                ErrorsResult.Add("ProductId ", "ProductId in cartLine can not be 0");
            }

            if(Successor != null)
                return Successor.HandleValidation(orderRequest);

            return ErrorsResult;
        }
    }
}
