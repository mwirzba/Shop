
using System.ComponentModel.DataAnnotations;

namespace Shop.Models.ProductDtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Name length can't be more than 8.")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(0,100000)]
        public decimal Price { get; set; }
        public byte CategoryId { get; set; }
        public virtual CategoryDto Category { get; set; }
    }
}
