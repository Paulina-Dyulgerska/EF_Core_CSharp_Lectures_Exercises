using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RealEstates.Models
{
   public class RealEstateProperty
    {
        public RealEstateProperty()
        {
            this.RealEstatePropertyTags = new HashSet<RealEstatePropertyTag>();
        }
        public int Id { get; set; }

        [Required]
        public int Size { get; set; }

        public int? Floor { get; set; } //tuk mi trqbwa null, zashtoto moje da ne znam na koj etaj e, i ako 
        //ne mi e pozwolen null, to trqbwa da pisha 0, koeto e realen etaj i e greshna informaciq.
        //za TotalNumberOfFloors vaji syshtoto.

        public int? TotalNumberOfFloors { get; set; }

        //ako si krystq pravilno propertytata ne mi trqbwat tezi attributi, EF sam shte se seti i shte gi napravi
        //[ForeignKey("District")]
        [ForeignKey(nameof(District))]
        public int DistrictId { get; set; }
        //[ForeignKey("DistrictId")] //ne e nujno towa, EF sam se seshta.
        public virtual District District { get; set; }

        public int? Year { get; set; }

        //ako si krystq pravilno propertytata ne mi trqbwat tezi attributi, EF sam shte se seti i shte gi napravi
        [ForeignKey("PropertyType")]
        public int PropertyTypeId { get; set; }
        public virtual PropertyType PropertyType { get; set; }

        //BuildingType mojeshe da e string i da si pisha v nego typovete sgradi, no togawa shtqh da imam
        //mnogo ednakvi zapisi v DB-a, a towa narushawa pricipa na normalisation-a.
        //oswen towa, utre ako promenq neshto po daden type, az trqbwa da go smenq na 10000000 mesta tazi promqna,
        //zashtoto v tablicata shte imam mnogo zapisi na nego. Zatowa, vinagi, kogato imam neshto, koeto se
        //povtarq kato zapis v DB-a, towa neshto da go iznasqm v otdelna tablica!!!! Towa napravih tuk s
        //tozi nov class BuildingType, kato vyzmojnite varianti na type na sgradi shte se pazqt v otdelna tablica
        //v DB-a, a v RealEstateProperty-to shte se pazi samo id-to na iskaniq type!
        //ako si krystq pravilno propertytata ne mi trqbwat tezi attributi, EF sam shte se seti i shte gi napravi
        [ForeignKey("BuildingType")]
        public int BuildingTypeId { get; set; }
        public virtual BuildingType BuildingType { get; set; }

        public int Price { get; set; }

        //[NotMapped] //ne e nujno da go kazwam, to e qsno, che ne se mappva v DB-a!!!
        public virtual ICollection<RealEstatePropertyTag> RealEstatePropertyTags { get; set; }

    }
}
