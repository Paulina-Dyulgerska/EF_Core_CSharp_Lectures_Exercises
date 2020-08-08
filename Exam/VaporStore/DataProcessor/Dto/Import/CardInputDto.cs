using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class CardInputDto
    {
        [Required]
        [RegularExpression("[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}")]
        public string Number { get; set; }

        [Required]
        [RegularExpression("[0-9]{3}")]
        public string Cvc { get; set; }

        [Required]
        [RegularExpression("Debit|Credit")] // !"^Debit|Credit$"
        public string Type { get; set; }

        //Number – text, which consists of 4 pairs of 4 digits, separated by spaces(ex. “1234 5678 9012 3456”) (required)
        //Cvc – text, which consists of 3 digits(ex. “123”) (required)
        //Type – enumeration of type CardType, with possible values(“Debit”, “Credit”) (required)
        //UserId – integer, foreign key(required)
        //User – the card’s user(required)
        //Purchases – collection of type Purchase
    }
}