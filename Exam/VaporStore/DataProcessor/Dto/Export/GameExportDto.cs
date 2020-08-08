using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Game")]
    public class GameExportDto
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        public string Genre { get; set; }

        public string Price { get; set; }

        [XmlIgnore]
        public decimal PriceDecimal { get; set; }

        //        <Game title = "Counter-Strike: Global Offensive" >
        //          < Genre > Action </ Genre >
        //          < Price > 12.49 </ Price >
        //        </ Game >
    }
}
