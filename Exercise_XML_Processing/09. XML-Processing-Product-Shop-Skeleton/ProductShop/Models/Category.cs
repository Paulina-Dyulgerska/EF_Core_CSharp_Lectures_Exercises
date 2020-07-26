namespace ProductShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    public class Category
    {
        public Category()
        {
            this.CategoryProducts = new HashSet<CategoryProduct>();
        }

        [Key]
        [XmlElement("id")]
        public int Id { get; set; }

        [Required]
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlIgnore]
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
