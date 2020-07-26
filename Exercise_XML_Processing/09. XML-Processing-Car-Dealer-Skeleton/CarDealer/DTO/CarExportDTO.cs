using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("car")]
   public class CarExportDTO
    {

        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }
    }
}
