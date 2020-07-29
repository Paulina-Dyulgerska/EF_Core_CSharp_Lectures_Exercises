using PetStore.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Gender
    {
        public Gender()
        {
            this.Pets = new HashSet<Pet>();
        }

        public int Id { get; set; }

        [Required, MinLength(GlobalConstants.NamesMinLenght), MaxLength(GlobalConstants.GeneralMaxLenght)]
        public string Type { get; set; } //male, female

        public virtual ICollection<Pet> Pets { get; set; }
    }
}