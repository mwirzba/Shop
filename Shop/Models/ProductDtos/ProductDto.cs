

using System.ComponentModel.DataAnnotations;

namespace Shop.Models.ProductDtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public byte CategoryId { get; set; }
        public virtual CategoryDto Category { get; set; }
    }
}
