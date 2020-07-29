using PetStore.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetStore.Models
{
    public class ClientProduct
    {
        [Required]
        //[ForeignKey("Client")]
        public string ClientId { get; set; }
        public virtual Client Client { get; set; }

        [Required]
        //[ForeignKey("Product")]
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        [Range(GlobalConstants.RangeQuantityMinValue, GlobalConstants.RangeQuantityMaxValue)] 
        public int Quantity { get; set; }

        [Required]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

    }
}
