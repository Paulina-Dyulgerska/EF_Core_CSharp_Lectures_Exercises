using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Users")]
    public class AllUsersDTO
    {
        //[XmlElement("count")]
        //public int Count { get => this.Users.Count; set { } } //v Judje iskashte samo 10 da vzema, no da
        //izkarwam Count-a na vsichki useri v DB tablicata, a towa mi 
        //izkarwashe 10. No towa e po-prawilniqt nachin, zashtoto za da imam counta na vsichki se nalaga sega
        //otvyn da setva propertyto Count kato predi towa prebroq kolko sa wsichki useri w DB tablicata.
        //Mnogo po-elegantno e towa da e calculated property i da mi vryshta kolko usera imam v predstavqnata 
        //collection, no Judge ne iska towa.

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlElement("users")]
        public AllusersListDTO Users { get; set; }
    } 
}
