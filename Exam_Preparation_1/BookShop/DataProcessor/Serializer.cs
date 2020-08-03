namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {

            //Select all authors along with their books.Select their name in format first name + ' ' + last name.
            //For each book select its name and price formatted to the second digit after the decimal 
            //point. Order the books by price in descending order. Finally sort all authors by book 
            //count descending and then by author full name.
            //NOTE: Before the orders, materialize the query(This is issue by Microsoft in InMemory database library)!!!
            
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Ignore, //!!!!!
            };

            var auhors = context.Authors
                .Select(a => new AuthorExportDTO
                {
                    AuthorName = a.FirstName + ' ' + a.LastName,
                    Books = a.AuthorsBooks
                      .Select(ab => new AuthorExportBookDTO
                      {
                          BookName = ab.Book.Name,
                          BookRealPrice = ab.Book.Price,
                      })
                      .OrderByDescending(ab => ab.BookRealPrice)
                      .ToArray(),
                })
                .OrderByDescending(a => a.Books.Count())
                .ThenBy(a => a.AuthorName)
                .ToArray();

            var json = JsonConvert.SerializeObject(auhors, jsonSettings);

            return json;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}