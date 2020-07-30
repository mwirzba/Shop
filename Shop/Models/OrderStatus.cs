using System.Collections.Generic;


namespace Shop.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
