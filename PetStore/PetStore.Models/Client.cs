using PetStore.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Client
    {
        public Client()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Pets = new HashSet<Pet>();

            this.ClientProducts = new HashSet<ClientProduct>();
        }

        public string Id { get; set; }

        [Required, MinLength(GlobalConstants.UserNameMinLenght), MaxLength(GlobalConstants.GeneralMaxLenght)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, MinLength(GlobalConstants.EmailMinLenght), MaxLength(GlobalConstants.GeneralMaxLenght)]
        public string Email { get; set; }

        [Required, MinLength(GlobalConstants.NamesMinLenght), MaxLength(GlobalConstants.GeneralMaxLenght)]
        public string FirstName { get; set; }

        [Required, MinLength(GlobalConstants.NamesMinLenght), MaxLength(GlobalConstants.GeneralMaxLenght)]
        public string LastName { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }

        public virtual ICollection<ClientProduct> ClientProducts { get; set; }
    }
}