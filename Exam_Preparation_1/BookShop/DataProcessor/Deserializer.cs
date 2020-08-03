namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var result = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<BookImportDTO>), new XmlRootAttribute("Books"));

            var books = new List<BookImportDTO>();

            using (var stream = new StringReader(xmlString))
            {
                books = (List<BookImportDTO>)serializer.Deserialize(stream);
            };

            var booksToAdd = new List<Book>();

            foreach (var book in books)
            {
                var hasValidDate = DateTime.TryParseExact(book.PublishedOn,
                    "MM/dd/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime dateTime);

                if (!IsValid(book) || !hasValidDate)
                {
                    result.AppendLine(ErrorMessage);
                }
                else
                {
                    var currentBook = new Book
                    {
                        Name = book.Name,
                        Genre = (Genre)book.Genre,
                        Pages = book.Pages,
                        Price = book.Price,
                        PublishedOn = dateTime.Date,
                    };

                    booksToAdd.Add(currentBook);

                    result.AppendLine(String.Format(SuccessfullyImportedBook, currentBook.Name, currentBook.Price));
                }
            }

            context.Books.AddRange(booksToAdd);

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var result = new StringBuilder();

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new DefaultNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Ignore,
            };

            var authors = JsonConvert.DeserializeObject<List<AuthorImportDTO>>(jsonString, jsonSettings);

            var authorsToAdd = new List<Author>();

            foreach (var author in authors)
            {
                //s tova proverqwam i kakwo veche ima v DB-a i kakwo imam prigotveno za zapis v DB-a, no oshte
                //nekacheno v neq!!!! Taka se pazq ot 2 strani i taka e prawilno!!!! Zashtoto ne znam dali
                //veche nqmam zapisi v DB-a, kogato trygvam da importvam moite!!! Kakto i ne znam dali v tekushto
                //importvanite zapisi nqmam povtoreniq na avtorski e-mail!!!!
                var hasExistingEmail = authorsToAdd.Any(x => x.Email == author.Email) ||
                                        context.Authors.Any(x => x.Email == author.Email);
                if (!IsValid(author) || hasExistingEmail)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var currentAuthor = new Author
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Email = author.Email,
                    Phone = author.Phone,
                };

                foreach (var book in author.Books.Distinct()) //Da vnimavam za towa!!!!!
                {
                    if (!book.Id.HasValue) //s tova si proverqwam dali id-to e null, no
                        //na praktika mi e izlishno towa, zashtoto akok id-to e null
                        //to v contexta nqma da ima nito edna kniga s takowa id i currentBook
                        //dolu, syshto shte e null!!!
                    {
                        continue;
                    }

                    var currentBook = context.Books.FirstOrDefault(x => x.Id == book.Id);

                    if (currentBook == null)
                    {
                        continue;
                    }

                    var authorBook = new AuthorBook
                    {
                        Author = currentAuthor,
                        Book = currentBook,
                    };

                    //moga da pisha edno ot tezi dve neshta:
                    currentAuthor.AuthorsBooks.Add(authorBook); //ako imam dolniq red, tozi ne mi trqbwa
                    context.AuthorsBooks.Add(authorBook); //raboti i bez towa ako imam gorniq red, 
                    //zashtoto avtomatichno sa nalivat Mapping Tablicite!!!!
                }

                if (currentAuthor.AuthorsBooks.Count == 0)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                authorsToAdd.Add(currentAuthor);

                result.AppendLine(String.Format(SuccessfullyImportedAuthor,
                    $"{currentAuthor.FirstName} {currentAuthor.LastName}",
                    currentAuthor.AuthorsBooks.Count));
            }

            context.Authors.AddRange(authorsToAdd);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
