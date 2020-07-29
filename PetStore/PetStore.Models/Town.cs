using PetStore.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace PetStore.Models
{
   public class Town
    {
        public int Id { get; set; }

        [Required, MinLength(GlobalConstants.NamesMinLenght), MaxLength(GlobalConstants.GeneralMaxLenght)]
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
