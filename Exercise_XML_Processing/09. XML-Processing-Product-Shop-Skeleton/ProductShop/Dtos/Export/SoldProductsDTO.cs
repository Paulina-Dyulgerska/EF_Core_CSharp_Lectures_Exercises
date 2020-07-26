using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
  public class SoldProductsDTO
    {
        [XmlElement("Product")]
        public List<ProductInExportUserWithSoldProductsDTO> Products { get; set; }

        //[XmlIgnore]
        //public int Count { get => this.Products.Count; set { } }
        public int Count => this.Products.Count; 
    }
}
