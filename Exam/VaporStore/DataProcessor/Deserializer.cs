namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            //•	If any validation errors occur(such as if a Price is negative,
            //a Name/ ReleaseDate / Developer / Genre is missing, Tags are missing or empty), 
            //do not import any part of the entity and append an error message to the method output.
            //•	Dates are always in the format “yyyy - MM - dd”. Do not forget to use CultureInfo.InvariantCulture!
            //•	If a developer / genre / tag with that name doesn’t exist, create it.
            //•	If a game is invalid, do not import its genre, developer or tags.

            var result = new StringBuilder();

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore, // !!! 
                Culture = CultureInfo.InvariantCulture,
                Formatting = Formatting.Indented,
            };

            var gamesDtos = JsonConvert.DeserializeObject<List<GameInputDto>>(jsonString, jsonSettings);
            var gamesToAdd = new List<Game>();
            var tagsToAdd = new List<Tag>();
            var genresToAdd = new List<Genre>();
            var developersToAdd = new List<Developer>();

            foreach (var gameDto in gamesDtos)
            {
                var hasValidReleaseDate = DateTime.TryParseExact(gameDto.ReleaseDate,
                      "yyyy-MM-dd",
                      CultureInfo.InvariantCulture,
                      DateTimeStyles.None,
                      out DateTime releaseDate);

                if (!IsValid(gameDto) || gameDto.Price < 0 || gameDto.Tags.Length <= 0 || !hasValidReleaseDate)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var currentGame = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = releaseDate,

                };

                var gameDeveloper = developersToAdd.FirstOrDefault(x => x.Name == gameDto.Developer);
                if (gameDeveloper == null)
                {
                    gameDeveloper = new Developer
                    {
                        Name = gameDto.Developer,
                    };
                    developersToAdd.Add(gameDeveloper);
                }
                currentGame.Developer = gameDeveloper;

                var gameGenre = genresToAdd.FirstOrDefault(x => x.Name == gameDto.Genre);
                if (gameGenre == null)
                {
                    gameGenre = new Genre
                    {
                        Name = gameDto.Genre,
                    };
                    genresToAdd.Add(gameGenre);
                }
                currentGame.Genre = gameGenre;

                foreach (var tag in gameDto.Tags)
                {
                    var currentTag = tagsToAdd.FirstOrDefault(x => x.Name == tag);

                    if (currentTag == null)
                    {
                        currentTag = new Tag
                        {
                            Name = tag
                        };
                        tagsToAdd.Add(currentTag);
                    }

                    var gameTag = new GameTag
                    {
                        Game = currentGame,
                        Tag = currentTag,
                    };

                    currentGame.GameTags.Add(gameTag);
                }

                gamesToAdd.Add(currentGame);

                result.AppendLine($"Added {currentGame.Name} ({currentGame.Genre.Name}) with " +
                    $"{currentGame.GameTags.Count} tags");
            }

            context.Tags.AddRange(tagsToAdd);
            context.Developers.AddRange(developersToAdd);
            context.Genres.AddRange(genresToAdd);
            context.Games.AddRange(gamesToAdd);

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            //•	If any validation errors occur(such as invalid full name, too short / long username, 
            //missing email, too low / high age, incorrect card number / CVC, no cards, etc.), 
            //do not import any part of the entity and append an error message to the method output.
            //•	If any validation errors occur with card entity(such as invalid number / CVC, invalid Type)
            //you should not import any part of the User entity holding this card and append an error 
            //message to the method output. 

            var result = new StringBuilder();

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore, // !!! 
                Culture = CultureInfo.InvariantCulture,
                Formatting = Formatting.Indented,
            };

            var usersDtos = JsonConvert.DeserializeObject<List<UserInputDto>>(jsonString, jsonSettings);

            var usersToAdd = new List<User>();

            foreach (var userDto in usersDtos)
            {

                if (!IsValid(userDto) || userDto.Cards.Length <= 0)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var currentUser = new User
                {
                    Username = userDto.Username,
                    FullName = userDto.FullName,
                    Age = userDto.Age,
                    Email = userDto.Email,
                };

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        result.AppendLine("Invalid Data");
                        continue;
                    }

                    var currentCard = new Card
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.Cvc,
                        Type = (CardType)Enum.Parse(typeof(CardType), cardDto.Type),
                    };

                    currentUser.Cards.Add(currentCard);
                }

                if (currentUser.Cards.Count == 0)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                usersToAdd.Add(currentUser);

                result.AppendLine($"Imported {currentUser.Username} with {currentUser.Cards.Count} cards");
            }

            context.Users.AddRange(usersToAdd);

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            //•	If there are any validation errors, do not import any part of the entity and append an 
            //error message to the method output.
            //•	Dates will always be in the format: “dd / MM / yyyy HH: mm”. Do not forget to use 
            //CultureInfo.InvariantCulture!

            var result = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<PurchaseInputDto>),
                new XmlRootAttribute("Purchases"));

            var purchasesDtos = new List<PurchaseInputDto>();

            using (var stream = new StringReader(xmlString))
            {
                purchasesDtos = (List<PurchaseInputDto>)serializer.Deserialize(stream);
            }

            var purchasesToAdd = new List<Purchase>();

            foreach (var purchaseDto in purchasesDtos)
            {
                var hasValidPurchaseDate = DateTime
                    .TryParseExact(purchaseDto.Date,
                        "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime purchaseDate);

                if (!IsValid(purchaseDto) || !hasValidPurchaseDate)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var currentPurchase = new Purchase
                {
                    Type = (PurchaseType)Enum.Parse(typeof(PurchaseType), purchaseDto.Type),
                    Date = purchaseDate,
                    ProductKey = purchaseDto.ProductKey,
                     
                };

                var currentCard = context.Cards.FirstOrDefault(x => x.Number == purchaseDto.Card);
                if (currentCard == null)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }
                currentPurchase.Card = currentCard;
                var username = context.Users.FirstOrDefault(x => x.Cards.Any(x => x.Number == purchaseDto.Card))
                    .Username;

                var currentGame = context.Games.FirstOrDefault(x => x.Name == purchaseDto.Game);
                if (currentGame == null)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }
                currentPurchase.Game = currentGame;

                purchasesToAdd.Add(currentPurchase);

                result.AppendLine($"Imported {currentPurchase.Game.Name} for {username}");
            }

            context.Purchases.AddRange(purchasesToAdd);

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