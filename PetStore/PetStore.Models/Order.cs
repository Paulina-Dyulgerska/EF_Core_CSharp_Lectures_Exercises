using PetStore.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace PetStore.Models
{
    public class Order
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ClientProducts = new HashSet<ClientProduct>();
        }

        public string Id { get; set; } //da proverq dali shte mi gi sloji na NULL????? Ami ne mi gi sloji Null!
        //t.e. nqma nujda da pisha [Required] za nikoe property s ime Id!!!! Nito e nijnod a pisha [Key]

        [Required]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        [Required, MinLength(GlobalConstants.GeneralMinLenght), MaxLength(GlobalConstants.GeneralMaxLenght)]
        public string Address { get; set; }

        public string Notes { get; set; }

        public virtual ICollection<ClientProduct> ClientProducts { get; set; }

        //[NotMapped] //TODO - mapva se v DB-a i kakwo se zapisva tam!
        public decimal TotalPrice { get => this.ClientProducts.Sum(cp => cp.Product.Price * cp.Quantity); set { } }

    }
}
