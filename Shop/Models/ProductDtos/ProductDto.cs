

using System.ComponentModel.DataAnnotations;

namespace Shop.Models.ProductDtos
{
    public class ProductDto
    {
        public int Id { get; set; }


        [Required]
        [StringLength(20,MinimumLength = 2)]
        public string Name { get; set; }
        public string Description { get; set; }


        [Required]
        [Range(1, 100000)]
        public decimal Price { get; set; }
        public byte CategoryId { get; set; }
        public virtual CategoryDto Category { get; set; }
    }
}
