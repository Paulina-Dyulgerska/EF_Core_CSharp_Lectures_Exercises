using BookShop.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class BookImportDTO
    {
        private const string decimalMaxValue = "79228162514264337593543950335";
        private const string priceMinValue = "0.01";
        private decimal price;

        [Required, MinLength(3), MaxLength(30)]
        public string Name { get; set; }

        [Required, Range(1, 3)]
        public int Genre { get; set; }

        //[Required]
        //public decimal Price
        //{
        //    get => this.price;
        //    private set
        //    {
        //        if (value < 0.01m || value > Decimal.MaxValue)
        //        {
        //            //throw new ArgumentException("Invalid data!");
        //            return;
        //        }
        //        this.price = value;
        //    }
        //}

        [Required, Range(typeof(decimal), priceMinValue, decimalMaxValue)]
        public decimal Price { get; set; }

        [Required, Range(50, 5000)]
        public int Pages { get; set; }

        [Required]
        //PublishedOn - date and time(required)
        public string PublishedOn { get; set; }
    }
}
