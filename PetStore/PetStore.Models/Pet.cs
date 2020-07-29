using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetStore.Models
{
    public class Pet
    {
        public Pet()
        {
            this.Id = Guid.NewGuid().ToString();
            //generira mi se avtomatichno string za GUID i s nego si
            //raboti konkretnata instanciq na classa Pet!!!! t.e. instanciqta shte se zapishe pod tozi unikalen
            //za neq GUID string za Id v tablicata Pets v DB-a!!!!.
            //towa predstawlqwa 1 GUID: 087a2692-8c45-4405-9dde-f3e09302d0bf.
        }

        public string Id { get; set; }
        //shte go pravq Id-to da e GUID, zashtoto e mnogo po-trudno 
        //po-advanced user da se seti kakwo e Id-to na sledwashtiqt red ot DB tablicata mi, otkolkoto ako go 
        //zapisa pod 1, 2 i 3..... GUID se generira v constructora.

        [Required, MinLength(GlobalConstants.NamesMinLenght), MaxLength(GlobalConstants.GeneralMaxLenght)]
        public string Name { get; set; }

        [Required]
        public int GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        [Required]
        [Range(GlobalConstants.RangeAgeMinValue, GlobalConstants.RangeAgeMaxValue)]
        public byte Age { get; set; }

        [Required]
        public int BreedId { get; set; }
        public virtual Breed Breed { get; set; }

        [Required]
        public bool IsSold { get; set; }

        [Required]
        [Range(GlobalConstants.RangePriceMinValue, Double.MaxValue)]
        public decimal Price { get; set; }

        public string ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
