using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstates.Models
{
    public class RealEstatePropertyTag
    {
        //[ForeignKey("RealEstateProperty")] 
        //ako si krystq pravilno propertytata ne mi trqbwat tezi attributi, EF sam shte se seti i shte gi napravi
        public int RealEstatePropertyId { get; set; }
        public virtual RealEstateProperty RealEstateProperty { get; set; }

        //[ForeignKey("Tag")]
        //ako si krystq pravilno propertytata ne mi trqbwat tezi attributi, EF sam shte se seti i shte gi napravi
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}