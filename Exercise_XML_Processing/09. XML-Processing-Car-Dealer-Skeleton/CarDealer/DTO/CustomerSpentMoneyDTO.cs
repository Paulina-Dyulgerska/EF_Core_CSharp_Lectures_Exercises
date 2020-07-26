using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("customer")]
    public class CustomerSpentMoneyDTO
    {
        [XmlAttribute("full-name")]
        public string FullName { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }  

        [XmlIgnore]
        public List<CarWithPartsExportDTO> Cars { get; set; }
    }
}
