﻿

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<CartLine> CartLines { get; set; }

        //public override bool Equals(object obj)
        //{
        //    var cmpProduct = (Product)obj;
        //    if (Name == cmpProduct.Name 
        //        && Description == cmpProduct.Description
        //        && Price == cmpProduct.Price
        //        && CategoryId == cmpProduct.CategoryId)
        //        return true;
        //    return false;
        //}

    }
}
