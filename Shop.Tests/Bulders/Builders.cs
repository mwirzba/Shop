using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Tests.Bulders
{
    public static class A
    {
        public static CategoryBuilder Category => new CategoryBuilder();
        public static CategoryDtoBuilder CategoryDto => new CategoryDtoBuilder();
        public static CartLineBuilder Cartline => new CartLineBuilder();
        public static CartLineDtoBuilder CartlineDto => new CartLineDtoBuilder();
        public static Userbuilder User => new Userbuilder();
        public static ProductBuilder Product => new ProductBuilder();
        public static ProductDtoBuilder ProductDto => new ProductDtoBuilder();
        public static CartLineRequestBuilder CartlineRequest => new CartLineRequestBuilder();
    }

    public static class An
    {
        public static OrderBuilder Order => new OrderBuilder();
        public static OrderDtoBuilder OrderDto => new OrderDtoBuilder();
        public static OrderRequestBuilder OrderRequest => new OrderRequestBuilder();
        public static OrderStatusBuilder OrderStatus => new OrderStatusBuilder();
        public static OrderStatusDtoBuilder OrderStatusDto => new OrderStatusDtoBuilder();
    }


}
