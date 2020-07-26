using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstates.Models
{
    public class Tag
    {
        public Tag()
        {
            this.RealEstatePropertyTags = new HashSet<RealEstatePropertyTag>();
        }
        public int Id { get; set; }

        [Required] //zadyljitelno e, inache taq tablica za kakwo q prawq?
        public string Name { get; set; }

        public string Description { get; set; }

        //v tazi collection shte si dyrja vsichki imoti ot tozi Tag i posle moga lesno da si gi vzimam!
        public virtual ICollection<RealEstatePropertyTag> RealEstatePropertyTags { get; set; }
    }
}