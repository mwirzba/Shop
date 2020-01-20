
using System.ComponentModel.DataAnnotations;

namespace Shop.Models.ProductDtos
{
    public class CategoryDto
    {
        public byte Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
