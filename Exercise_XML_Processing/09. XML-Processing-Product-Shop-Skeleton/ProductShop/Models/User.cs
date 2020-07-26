namespace ProductShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    public class User
    {
        public User()
        {
            this.ProductsSold = new HashSet<Product>();
            this.ProductsBought = new HashSet<Product>();
        }

        [Key]
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [Required]
        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }
        
        [XmlIgnore]
        public virtual ICollection<Product> ProductsSold { get; set; }
        
        [XmlIgnore]
        public virtual ICollection<Product> ProductsBought { get; set; }
    }
}