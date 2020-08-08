using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Import
{
    [XmlType("Purchase")]
    public class PurchaseInputDto
    {
        //<Purchase title = "Dungeon Warfare 2" >
        //< Type > Digital </ Type >
        //< Key > ZTZ3 - 0D2S-G4TJ</Key>
        //<Card>1833 5024 0553 6211</Card>
        //<Date>07/12/2016 05:49</Date>

        [XmlAttribute("title")]
        [Required]
        public string Game { get; set; }

        [Required]
        [RegularExpression("Retail|Digital")] ///!!!!!!!!!!!!!
        public string Type { get; set; }

        [Required]
        [XmlElement("Key")]
        [RegularExpression("^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        public string ProductKey { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        [RegularExpression("[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}")] ///!!!!!!!!
        public string Card { get; set; }

        //Type – enumeration of type PurchaseType, with possible values(“Retail”, “Digital”) (required)
        //ProductKey – text, which consists of 3 pairs of 4 uppercase Latin letters and digits, 
        ////separated by dashes(ex. “ABCD-EFGH-1J3L”) (required)
        //Date – Date(required)
        //CardId – integer, foreign key(required)
        //Card – the purchase’s card(required)
        //GameId – integer, foreign key(required)
        //Game – the purchase’s game(required)
    }
}
