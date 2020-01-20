using System.Collections.Generic;


namespace Shop.Models
{
    public class Category
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
