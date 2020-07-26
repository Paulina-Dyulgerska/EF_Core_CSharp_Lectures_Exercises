using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("partId")]
    public class PartsCarImportDTO
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
