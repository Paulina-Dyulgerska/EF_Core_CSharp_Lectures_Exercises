using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstates.Models
{
    public class Tag
    {
        public Tag()
        {
            this.Tags = new HashSet<RealEstatePropertyTag>();
        }
        public int Id { get; set; }

        [Required] //zadyljitelno e, inache taq tablica za kakwo q prawq?
        public string Name { get; set; }

        public string Description { get; set; }

        //v tazi collection shte si dyrja vsichki imoti ot tozi Tag i posle moga lesno da si gi vzimam!
        //[NotMapped] //ne e nujno da go kazwam, to e qsno, che ne se mappva v DB-a!!!
        public virtual ICollection<RealEstatePropertyTag> Tags { get; set; }
    }
}