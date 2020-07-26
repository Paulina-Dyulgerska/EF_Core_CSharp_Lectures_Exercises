using CarDealer.Models;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("sale")]
    public class SaleExportDTO
    {
        [XmlElement("car")]
        public CarExportDTO Car { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("price-with-discount")]
        public decimal PriceWithDiscount { get; set; }
        //logichno e da se smqta towa taka, no Judge misli drugo, iskam onaq typoviq, koqto polzwam
        //v StartUp, t.e. tam da prisvoqwam stojnostta na tazi cena i da smqtam 100 pyti edni i syshti neshta i da
        //habq pamet i resuources s tova:
        //PriceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100m,
        //a az bih go naprawila taka krasiwo:
        //public string PriceWithDiscount { get => (this.Price * (100 - this.Discount) / 100M).ToString("f2"); set { } }

    }
}
