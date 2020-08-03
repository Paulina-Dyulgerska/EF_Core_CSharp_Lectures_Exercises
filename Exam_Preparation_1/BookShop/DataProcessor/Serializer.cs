namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
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
                    NamingStrategy = new DefaultNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                //NullValueHandling = NullValueHandling.Ignore, //!!!!!
            };

            var authors = context.Authors
                .Select(a => new AuthorExportDTO
                {
                    AuthorName = a.FirstName + ' ' + a.LastName,
                    Books = a.AuthorsBooks
                    .Select(ab => ab.Book)
                    .OrderByDescending(ab => ab.Price)
                    .Select(ab => new AuthorExportBookDTO
                    {
                        BookName = ab.Name,
                        BookPrice = ab.Price.ToString("f2"),
                    })
                    .ToArray(),
                })
                .ToArray()
                .OrderByDescending(a => a.Books.Count())
                .ThenBy(a => a.AuthorName)
                .ToArray();

            var json = JsonConvert.SerializeObject(authors, jsonSettings);

            return json;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            // Export top 10 oldest books that are published before the given date and are of type science.
            // For each book select its name, date (in format "d") and pages.Sort them by pages in 
            // descending order and then by date in descending order.
            //NOTE: Before the orders, materialize the query (This is issue by Microsoft in InMemory database library)!!!

            var books = context.Books
                //.Where(b => b.PublishedOn < date && b.Genre == (Genre)Enum.Parse(typeof(Genre), "Science"))
                .Where(b => b.PublishedOn < date && b.Genre == Genre.Science)
                .Select(b => new BookExportDTO
                {
                    Date = b.PublishedOn.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    //Date = b.PublishedOn.ToString("d", CultureInfo.InvariantCulture),
                    Name = b.Name,
                    Pages = b.Pages,
                })
                .ToArray()
                .OrderByDescending(b => b.Pages)
                .ThenByDescending(b => b.Date)
                .Take(10)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(BookExportDTO[]), new XmlRootAttribute("Books"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var xml = new StringBuilder();

            using (var writer = new StringWriter(xml))
            {
                serializer.Serialize(writer, books, namespaces);
            };

            return xml.ToString().TrimEnd();
        }
    }
}