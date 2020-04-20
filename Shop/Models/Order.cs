using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /*
        public virtual OrderInformations OrderInformations { get; set; }
        public long OrderInformationsId { get; set; }*/
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public bool GiftWrap { get; set; }
        public string Status { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public ICollection<CartLine> CartLines { get; set; }

    }
}
