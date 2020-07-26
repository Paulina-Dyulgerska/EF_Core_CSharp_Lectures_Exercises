using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace CarDealer.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [ForeignKey("Car")]
        [XmlElement("carId")]
        public int CarId { get; set; }
        public Car Car { get; set; }

        [ForeignKey("Customer")]
        [XmlElement("customerId")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }
    }
}
