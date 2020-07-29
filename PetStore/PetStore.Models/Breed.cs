using PetStore.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Breed
    {
        public Breed()
        {
            this.Pets = new HashSet<Pet>();
        }
        
        public int Id { get; set; }

        [Required, MinLength(GlobalConstants.NamesMinLenght), MaxLength(GlobalConstants.GeneralMaxLenght)]
        public string Type { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}