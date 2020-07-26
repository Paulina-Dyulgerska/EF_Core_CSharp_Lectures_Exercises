using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("car")]
    public class CarExportWithAttributesDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlIgnore]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }
    }
}
