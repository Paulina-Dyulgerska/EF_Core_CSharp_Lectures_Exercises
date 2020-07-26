namespace CarDealer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class Customer
    {
        public Customer()
        {
            this.Sales = new HashSet<Sale>();
        }

        public int Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("birthDate")]
        public DateTime BirthDate { get; set; }

        [XmlElement("isYoungDriver")]
        public bool IsYoungDriver { get; set; }

        [XmlIgnore]
        public virtual ICollection<Sale> Sales { get; set; }
    }
}