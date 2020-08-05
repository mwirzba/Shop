
using System.Collections.Generic;


namespace Shop.Dtos
{
    public class OrderDto
    {
        public long Id { get; set; }
        public ICollection<CartLineDto> CartLines { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public bool GiftWrap { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string UserName { get; set; }
    }
}
