using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("users")]
    public class AllusersListDTO
    {
        [XmlElement("User")]
        public List<UserWithSoldProductsWithCountDTO> Users { get; set; }

        [XmlIgnore]
        public int Count => this.Users.Count;
    }
}
