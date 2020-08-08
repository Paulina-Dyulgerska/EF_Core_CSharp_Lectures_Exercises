namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            //The given method in the project skeleton receives an array of genre names.Export all games 
            //in those genres, which have any purchases.For each genre, export its id, genre name, games 
            //and total players(total purchase count).For each game, export its id, name, developer, 
            //tags (separated by ", ") and total player count(purchase count). Order the games by player 
            //count(descending), then by game id(ascending).
            //Order the genres by total player count(descending), then by genre id(ascending) 

            //    var games = context.Genres
            //        .Include(x => x.Games)
            //        .ThenInclude(g => g.Purchases)
            //        .Include(x => x.Games)
            //        .ThenInclude(g => g.Developer)
            //        .Include(x => x.Games)
            //        .ThenInclude(g => g.GameTags)
            //        .ThenInclude(gt => gt.Tag)
            //        .ToList()
            //        .Where(x => genreNames.Contains(x.Name) == true)
            //        .Select(x => new
            //        {
            //            Id = x.Id,
            //            Genre = x.Name,
            //            Games = x.Games
            //                .Where(g => g.Purchases.Count > 0)
            //                .Select(g => new
            //                {
            //                    Id = g.Id,
            //                    Title = g.Name,
            //                    Developer = g.Developer.Name,
            //                    Tags = string.Join(", ", g.GameTags.Select(gt => gt.Tag.Name)),
            //                    Players = g.Purchases.Count
            //                })
            //                .OrderByDescending(g => g.Players)
            //                .ThenBy(g => g.Id)
            //                .ToList(),
            //            TotalPlayers = x.Games.SelectMany(g => g.Purchases).Count(),
            //        })
            //        .OrderByDescending(x => x.TotalPlayers)
            //        .ThenBy(x => x.Id)
            //        .ToList();

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore,
                Culture = CultureInfo.InvariantCulture,
                Formatting = Formatting.Indented,
                //DateFormatString = "dd-MM-yyyy",
            };

            var games = context.Genres
                .Where(genre => genreNames.Any(gn => gn == genre.Name))
                .ToArray()
                .Select(genre => new
                {
                    genre.Id,
                    Genre = genre.Name,
                    Games = genre.Games
                    .Where(g => g.Purchases.Count > 0)
                    .ToArray()
                    .Select(g => new
                    {
                        g.Id,
                        Title = g.Name,
                        Developer = g.Developer.Name,
                        Tags = String.Join(", ", g.GameTags.Select(gt => gt.Tag.Name)),
                        Players = g.Purchases.Count,
                    })
                    .OrderByDescending(g => g.Players)
                    .ThenBy(g => g.Id)
                    .ToArray(),
                    //TotalPlayers = g.Games.Select(g => g.Purchases.Count).Sum(), //NIKOGA DA NE PISHA TOWA TAKA
                    //a da si prvaq SelectMany!!!!! SelectMany izglajda collectiona i posle mu prilagam
                    //Count() i minava!!!!! Dokato towa sys Select - gyrmi!!!!! Taka se pravi:
                    TotalPlayers = genre.Games.SelectMany(g => g.Purchases).Count(),
                    //towa pravi proektion ot vsichki Purchases za daden genre i negovite games i gi prebroqwa!!!!
                    //t.e. vliza v genre, vzima vsqka edna igra, posle vzima vsqka purchase za tazi igra i gi
                    //redi tezi purchases edna sled druga za wsqka igra ot dadeniq genre. ot taka poluchenata
                    //collection, az vzimam kolko broq zapisa ima s Count()!!!!!
                })
                .OrderByDescending(genre => genre.TotalPlayers)
                .ThenBy(genre => genre.Id)
                .ToArray();

            var json = JsonConvert.SerializeObject(games, jsonSettings);

            return json.ToString().TrimEnd();
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            //Use the method provided in the project skeleton, which receives a purchase type as a string.
            //Export all users who have any purchases. For each user, export their username, purchases for 
            //that purchase type and total money spent for that purchase type.For each purchase, export its 
            //card number, CVC, date in the format "yyyy-MM-dd HH:mm"(make sure you use CultureInfo.InvariantCulture) 
            //and the game.For each game, export its title(name), genre and price.Order the users by 
            //total spent(descending), then by username(ascending).For each user, 
            //order the purchases by date(ascending).Do not export users, who don’t have any purchases.
            //Note: All prices must be in decimal without any formatting!

            //    var users = context.Users
            //        .Include(u => u.Cards)
            //        .ThenInclude(c => c.Purchases)
            //        .ThenInclude(p => p.Game)
            //        .ThenInclude(g => g.Genre)
            //        .ToList()
            //        .Where(u => u.Cards.SelectMany(c => c.Purchases).Count() > 0)
            //        .Select(u => new ExportUserPurchasesDto
            //        {
            //            Username = u.Username,
            //            Purchases = u.Cards.SelectMany(c => c.Purchases)
            //                .Where(p => p.Type.ToString() == storeType)
            //                .Select(p => new PurchasesDto
            //                {
            //                    Card = p.Card.Number,
            //                    Cvc = p.Card.Cvc,
            //                    Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
            //                    Game = new GameDto
            //                    {
            //                        Title = p.Game.Name,
            //                        Genre = p.Game.Genre.Name,
            //                        Price = p.Game.Price
            //                    }
            //                })
            //                .OrderBy(p => DateTime.Parse(p.Date)).ToList(),
            //            TotalMoneySpent = u.Cards.SelectMany(c => c.Purchases)
            //                .Where(p => p.Type.ToString() == storeType).Sum(p => p.Game.Price)
            //        })
            //        .Where(u => u.Purchases.Count > 0)
            //        .OrderByDescending(u => u.TotalMoneySpent)
            //        .ThenBy(u => u.Username)
            //        .ToList();

            var usersNotSorted = context.Users
                .Where(u => u.Cards.Any(c => c.Purchases.Count > 0))
                .ToArray()
                .Select(u => new UserExportDto
                {
                    Username = u.Username,
                    Purchases = context.Purchases
                    .Where(p => p.Card.User.Id == u.Id)
                    .Where(p => p.Type.ToString() == storeType)
                    .ToArray()
                    .Select(c => new PurchaseExportDto
                    {
                        Card = c.Card.Number,
                        Cvc = c.Card.Cvc,
                        Date = c.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new GameExportDto
                        {
                            Title = c.Game.Name,
                            Price = String.Format(c.Game.Price % 1 == 0 ? "{0:0}" : "{0:0.00}", c.Game.Price),
                            Genre = c.Game.Genre.Name,
                            PriceDecimal = c.Game.Price,
                        },
                        Type = c.Type.ToString(),
                    })
                    .ToArray(),
                })
                .ToArray();

            var users = usersNotSorted
                .Where(u => u.Purchases.Count() > 0)
                .Select(u => new UserExportDto
                {
                    Username = u.Username,
                    Purchases = u.Purchases.OrderBy(p => p.Date).ToArray(),
                    TotalSpent = u.Purchases.Select(p => p.Game.PriceDecimal).Sum(),
                })
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            var xml = new StringBuilder();

            var serializer = new XmlSerializer(typeof(UserExportDto[]), new XmlRootAttribute("Users"));
            var namespases = new XmlSerializerNamespaces();
            namespases.Add(string.Empty, string.Empty);

            using (var writer = new StringWriter(xml))
            {
                serializer.Serialize(writer, users, namespases);
            }
            return xml.ToString().TrimEnd();
        }
    }
}