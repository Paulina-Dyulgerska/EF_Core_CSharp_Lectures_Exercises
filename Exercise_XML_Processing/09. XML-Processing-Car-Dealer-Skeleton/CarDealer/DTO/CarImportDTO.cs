using CarDealer.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("Car")]
    public class CarImportDTO
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("TraveledDistance")]
        public long TravelledDistance { get; set; }

        [XmlIgnore]
        public virtual ICollection<Sale> Sales { get; set; }

        [XmlArray("parts")]
        public virtual List<PartsCarImportDTO> PartCars { get; set; }
    }
}
