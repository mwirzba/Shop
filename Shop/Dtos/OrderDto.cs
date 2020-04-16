
using System.Collections.Generic;


namespace Shop.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public ICollection<CartLineDto> CartLines { get; set; }
        public string Name { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public bool GiftWrap { get; set; }

        public string Status { get; set; }
        public string UserDto { get; set; }
    }
}
