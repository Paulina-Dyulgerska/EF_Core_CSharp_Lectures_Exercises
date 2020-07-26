namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Z.EntityFramework.Plus;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //da ne zabrawqm tozi using, zashtoto BookShopContext e IDisposable.
            //DbInitializer.ResetDatabase(db);

            //var input = int.Parse(Console.ReadLine());

            //Console.WriteLine($"{RemoveBooks(db)} books were deleted");

            var result = RemoveBooks(db);

            Console.WriteLine(result);
            //Console.WriteLine(result.Length);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var sb = new StringBuilder();

            //var ageRestrictionEnum = command[0].ToString().ToUpper() + command.Substring(1).ToLower();            
            var ageRestrictionEnum = command.ToLower();

            var books = context.Books
                .AsEnumerable() //za da mine v judge pravq tazi glupost, inache si e naj-pravilno s moq Enum.Parse!!!
                .Where(x => x.AgeRestriction.ToString().ToLower() == ageRestrictionEnum)
                //.Where(x => x.AgeRestriction == Enum.Parse<AgeRestriction>(ageRestrictionEnum, ignoreCase: true))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Books
                //.Where(x => x.EditionType == Enum.Parse<EditionType>("Gold") && x.Copies < 5000)
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x => new
                {
                    x.Title,
                    x.Price,
                })
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => new
                {
                    x.Title,
                })
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            //var categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            var categories = input
                    .ToLower()
                    .Split(new char[] { ' ', ',', '!', '?', '.', ':', ':' }, StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                .Where(x => x.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                .OrderBy(x => x.Title)
                .Select(x => new
                {
                    x.BookId,
                    x.Title,
                    //BookCats = x.BookCategories.Select(x=>x.Category.Name),
                })
                .ToList();

            //var books = context.Books
            //                    .Select(x => new
            //                    {
            //                        x.BookId,
            //                        x.Title,
            //                        BookCats = x.BookCategories.Select(x => x.Category.Name),
            //                    })
            //                    .Where(x => x.BookCats.Any(c => categories.Contains(c.ToLower())))
            //                    .OrderBy(x => x.Title)
            //                    .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
                //foreach (var c in book.BookCats)
                //{
                //    sb.AppendLine($"---{c}");
                //}
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var date = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(x => x.ReleaseDate < date)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new
                {
                    x.BookId,
                    x.Title,
                    x.EditionType,
                    x.Price,
                })
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var authors = context.Authors
                .Where(a => a.FirstName.ToLower().EndsWith(input.ToLower()))
                .ToList()
                .Select(a => a.FirstName == null ? a.LastName : $"{a.FirstName} {a.LastName}")
                .OrderBy(a => a);

            //        var authors = context.Books
            //.Where(x => x.Author.FirstName.ToLower().EndsWith(input.ToLower()))
            //.Select(x => x.Author.FirstName + ' ' + x.Author.LastName)
            //.Distinct()
            //.OrderBy(x => x)
            //.ToList();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            //var authors = context.Authors
            //    .Where(a => a.LastName.ToLower().StartsWith(input.ToLower()))
            //    .ToList()
            //    .Select(a => a.FirstName == null ? a.LastName : $"{a.FirstName} {a.LastName}")
            //    .OrderBy(a => a);

            var books = context.Books
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(b => new
                {
                    b.Title,
                    AuthorFirstName = b.Author.FirstName,
                    AuthorLastName = b.Author.LastName,
                })
                .ToList()
                .Select(b => $"{b.Title} ({b.AuthorFirstName} {b.AuthorLastName})");

            foreach (var book in books)
            {
                sb.AppendLine($"{book}");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var booksCount = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .ToList()
                .Count;

            return booksCount;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();

            var authors = context.Authors
                .Select(a => new
                {
                    AuthorName = a.FirstName + ' ' + a.LastName,
                    BookCopies = a.Books.Select(b => b.Copies).Sum(),
                })
                .OrderByDescending(b => b.BookCopies)
                .ToList();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.AuthorName} - {author.BookCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();

            var categories = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    CategoryBooksProfit = c.CategoryBooks.Select(b => b.Book.Price * b.Book.Copies).Sum(),
                })
                .OrderByDescending(c => c.CategoryBooksProfit)
                .ThenBy(c => c.CategoryName)
                .ToList();

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.CategoryName} ${category.CategoryBooksProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            //Get the most recent books by categories.The categories should be ordered by name alphabetically. 
            //    Only take the top 3 most recent books from each category -ordered by release date(descending).
            //    Select and print the category name, and for each book – its title and release year.

            var categories = context.Categories
                    .Select(c => new
                    {
                        CategoryName = c.Name,
                        CategoryBooks = c.CategoryBooks
                        .Select(b => new { b.Book.Title, b.Book.ReleaseDate })
                        .OrderByDescending(b => b.ReleaseDate)
                        .Take(3),
                    })
                    .OrderBy(c => c.CategoryName)
                    .ToList();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.CategoryBooks)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            //Increase the prices of all books released before 2010 by 5.

            var books = context.Books.Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            ////context.Database.ExecuteSqlRaw("DELETE FROM Books WHERE Copies < 4200"); //not working in judge

            return context.Books.Where(b => b.Copies < 4200).Delete(); //not working in judge

            //var books = context.Books.Where(b => b.Copies < 4200).ToArray();
            //var removedBooks = books.Length;
            //context.Books.RemoveRange(books);
            //context.SaveChanges();
            //return removedBooks; ////not working in judge

            //var booksForDelete = context.Books.Where(b => b.Copies < 4200).ToList();
            //context.RemoveRange(booksForDelete);
            //context.SaveChanges();
            ////return context.SaveChanges(); //not working in judge
            //return booksForDelete.Count; //not working in judge
        }
    }
}
