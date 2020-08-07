using Shop.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace Shop.Validation.ChainsOfResponsibility.Order
{
    public class ProductIdInEachCartLineUniqueValidator: ValidatorBase<OrderRequest>
    {
        public override Dictionary<string, string> HandleValidation(OrderRequest orderRequest)
        {
            var repeatedProductsId = orderRequest.CartLines.GroupBy(x => x.ProductId)
                                            .Where(g => g.Count() > 1)
                                            .Select(y => new { Element = y.Key, Counter = y.Count() })
                                            .ToList();

            if (repeatedProductsId != null)
            {
                ErrorsResult.Add("ProductId", "Same product can not be in multiple cartLines");
            }

            if (Successor != null)
                return Successor.HandleValidation(orderRequest);

            return ErrorsResult;
        }
    }
}
