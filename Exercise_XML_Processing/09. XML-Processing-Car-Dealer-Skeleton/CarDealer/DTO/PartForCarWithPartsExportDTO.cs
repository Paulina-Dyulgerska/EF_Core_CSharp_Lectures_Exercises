using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("part")]
    public class PartForCarWithPartsExportDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
