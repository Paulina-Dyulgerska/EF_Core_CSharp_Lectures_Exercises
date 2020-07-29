using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ClientProducts = new HashSet<ClientProduct>();
        }

        public string Id { get; set; }

        public int OfficialId { get; set; }

        [Required, MinLength(GlobalConstants.NamesMinLenght)]
        public string Name { get; set; }

        [Required]
        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }

        [Required]
        [Range(GlobalConstants.RangePriceMinValue, Double.MaxValue)]
        public decimal Price { get; set; }

        public virtual ICollection<ClientProduct> ClientProducts { get; set; }
    }
}
