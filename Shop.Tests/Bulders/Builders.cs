using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Tests.Bulders
{
    public static class A
    {
        public static CategoryBuilder Category => new CategoryBuilder();
        public static Userbuilder User => new Userbuilder();
        public static ProductBuilder Product => new ProductBuilder();
    }

    public static class An
    {
        public static OrderBuilder Order => new OrderBuilder();
    }


}
