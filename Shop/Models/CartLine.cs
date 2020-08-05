
namespace Shop.Models
{
    public class CartLine
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public long OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
