namespace ProductShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Xml.Serialization;

    public class Product
    {
        public Product()
        {
            this.CategoryProducts = new HashSet<CategoryProduct>();
        }

        [Key]
        [XmlElement("id")]
        public int Id { get; set; }

        [Required]
        [XmlElement("name")]
        public string Name { get; set; }

        [Required]
        [XmlElement("price")]
        public decimal Price { get; set; }

        [Required, ForeignKey("Seller")]
        [XmlElement("sellerId")]
        public int SellerId { get; set; }
        [XmlIgnore]
        public virtual User Seller { get; set; }

        [ForeignKey("Buyer")]
        [XmlElement("buyerId")]
        //[XmlElement(IsNullable = true)]
        //[XmlChoiceIdentifier("buyerId")]
        public int? BuyerId { get; set; }
        [XmlIgnore]
        public virtual User Buyer { get; set; }
        
        [XmlIgnore]
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}