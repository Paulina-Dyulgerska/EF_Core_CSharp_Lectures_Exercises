
namespace CarDealer.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Xml.Serialization;

    public class Part
    {
        public Part()
        {
            this.PartCars = new HashSet<PartCar>();
        }

        public int Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("quantity")]
        public int Quantity { get; set; }

        [ForeignKey("Supplier")]
        [XmlElement("supplierId")]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [XmlIgnore]
        public virtual ICollection<PartCar> PartCars { get; set; }
    }
}
