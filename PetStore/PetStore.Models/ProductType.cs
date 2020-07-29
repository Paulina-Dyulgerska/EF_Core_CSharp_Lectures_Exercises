using PetStore.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class ProductType
    {
        public ProductType()
        {
            this.Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [Required, MinLength(GlobalConstants.NamesMinLenght) ]
        public string Type { get; set; } //food, toy

        public virtual ICollection<Product> Products { get; set; }

    }
}