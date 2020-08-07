

namespace Shop.Dtos
{
    public class CartLineRequest
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
