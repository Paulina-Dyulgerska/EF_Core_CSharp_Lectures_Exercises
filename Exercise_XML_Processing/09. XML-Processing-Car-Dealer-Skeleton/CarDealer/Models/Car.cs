namespace CarDealer.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlType("car")]
    public class Car
    {
        public Car()
        {
            this.PartCars = new HashSet<PartCar>();
        }

        [XmlIgnore]
        public int Id { get; set; }

        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("travelled-distance")]
        public long TravelledDistance { get; set; }

        [NotMapped]
        [XmlIgnore]
        public decimal TotalCarPrice { get => this.PartCars.Sum(p => p.Part.Price); set { } }

        [XmlIgnore]
        public virtual ICollection<Sale> Sales { get; set; }

        [XmlIgnore]
        public virtual ICollection<PartCar> PartCars { get; set; }
    }
}