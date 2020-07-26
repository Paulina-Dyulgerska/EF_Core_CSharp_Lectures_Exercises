using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstates.Models
{
    public class PropertyType
    {
        public PropertyType()
        {
            this.RealEstateProperties = new HashSet<RealEstateProperty>();
        }

        public int Id { get; set; }

        [Required] //zadyljitelno e, inache taq tablica za kakwo q prawq?
        public string Name { get; set; }

        //v tazi collection shte si dyrja vsichki imoti ot tozi PropertyType i posle moga lesno da si gi vzimam!
        public virtual ICollection<RealEstateProperty> RealEstateProperties { get; set; }
    }
}