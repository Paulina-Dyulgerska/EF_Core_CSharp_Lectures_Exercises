using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("car")]
   public class CarWithPartsExportDTO
    {

        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        [XmlArrayItem("part")]
        public virtual List<PartForCarWithPartsExportDTO> PartCars { get; set; } = new List<PartForCarWithPartsExportDTO>();
    }
}
