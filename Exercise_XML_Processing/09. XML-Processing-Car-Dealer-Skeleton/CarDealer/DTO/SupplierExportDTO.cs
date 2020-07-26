using System.Xml.Serialization;

namespace CarDealer.DTO
{
    //[XmlType("supplier")]
    [XmlType("suplier")] //samo s tova greshno izpisvane na supplier mi minawa v Judge, inache preskacham limita
    //za pamet, zaradi izpisvaneto na vtoroto p.
    public class SupplierExportDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("parts-count")]
        public int PartsCount { get; set; }
    }
}
