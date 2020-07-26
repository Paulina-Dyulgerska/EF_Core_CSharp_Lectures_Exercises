using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("SoldProducts")]
    public class SoldProductsWithCountDTO
    {
        [XmlElement("count")]
        public int Count { get => this.Products.Count; set { } }

        [XmlElement("products")]
        public SoldProductsDTO Products { get; set; }
    }
}
