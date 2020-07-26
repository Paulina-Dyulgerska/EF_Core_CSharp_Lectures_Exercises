using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Category;
using ProductShop.DTOs.Product;
using ProductShop.DTOs.User;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private const string outputPath = "../../../Results";
        public static void Main()
        {
            EnsureOutputDirectoryExists(outputPath);

            InitializeMapper();

            var dbContext = new ProductShopContext();
            Console.WriteLine(ResetDatabase(dbContext));

            //var usersInputJson = File.ReadAllText("../../../Datasets/users.json");
            //ImportUsers(dbContext, usersInputJson);

            //var productsInputJson = File.ReadAllText("../../../Datasets/products.json");
            //ImportProducts(dbContext, productsInputJson);

            //var categoriesInputJson = File.ReadAllText("../../../Datasets/categories.json");
            //var result = ImportCategories(dbContext, categoriesInputJson);

            //var categoriesProductsInputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //var result = ImportCategoryProducts(dbContext, categoriesProductsInputJson);

            var result = GetUsersWithProducts(dbContext);

            Console.WriteLine(result);


        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Ignore,
            };

            var users = context.Users
                .Where(x => x.ProductsSold.Any(a => a.BuyerId != null))
                //.AsEnumerable() //Judge iska towa, za da mine.
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Age,
                    SoldProducts = new
                    {
                        Count = x.ProductsSold.Where(p => p.BuyerId != null).Count(),
                        Products = x.ProductsSold.Where(p => p.BuyerId != null)
                                    .Select(p => new
                                    {
                                        p.Name,
                                        p.Price,
                                    }),
                    }
                })
                .OrderByDescending(x => x.SoldProducts.Count) //s Order sled Where Judge mi gyrmi!
                .AsEnumerable();

            var usersView = new
            {
                usersCount = users.Count(),
                users,
            };

            //with DTOs:

            var json = JsonConvert.SerializeObject(usersView, jsonSettings);

            //File.WriteAllText($"{outputPath}/users-and-products.json", json); //Judge gyrmi s towa.

            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Include,
            };

            //var products = context.Categories
            //    .OrderByDescending(x => x.CategoryProducts.Count)
            //    .Select(x => new
            //    {
            //        category = x.Name,
            //        productsCount = x.CategoryProducts.Count,
            //        averagePrice = (x.CategoryProducts.Select(p => p.Product.Price).Sum() / x.CategoryProducts.Count).ToString("f2"),
            //        totalRevenue = x.CategoryProducts.Select(p => p.Product.Price).Sum().ToString("f2"),
            //    })
            //    .AsEnumerable();

            //with DTOs:
            var products = context.Categories
                .OrderByDescending(x => x.CategoryProducts.Count)
                .ProjectTo<CategoryWithProductsDTO>()
                .AsEnumerable();

            var json = JsonConvert.SerializeObject(products, jsonSettings);

            //File.WriteAllText($"{outputPath}/categories-by-products.json", json); //Judge gyrmi s towa.

            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Include,
            };

            //var products = context.Users
            //    .Where(x => x.ProductsSold.Any(a => a.BuyerId != null))
            //    .OrderBy(x => x.LastName)
            //    .ThenBy(x => x.FirstName)
            //    .Select(x => new
            //    {
            //        firstName = x.FirstName,
            //        lastName = x.LastName,
            //        soldProducts = x.ProductsSold
            //        .Where(p => p.BuyerId != null)
            //        .Select(p => new
            //        {
            //            Name = p.Name,
            //            Price = p.Price,
            //            BuyerFirstName = p.Buyer.FirstName,
            //            BuyerLastName = p.Buyer.LastName,
            //        }),
            //    })
            //  .AsEnumerable();

            //with DTOs:
            var products = context.Users
                .Where(x => x.ProductsSold.Any(a => a.BuyerId != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ProjectTo<UserWithSoldProductsDTO>()
                .AsEnumerable();

            var json = JsonConvert.SerializeObject(products, jsonSettings);

            //File.WriteAllText($"{outputPath}/users-sold-products.json", json); //Judge gyrmi s towa.

            return json;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Include,
            };

            //var products = context.Products
            //    .Where(x => x.Price >= 500m && x.Price <= 1000m)
            //    .OrderBy(x => x.Price)
            //    .Select(x => new
            //    {
            //        x.Name,
            //        x.Price,
            //        //Seller = (x.Seller.FirstName !=null ? x.Seller.FirstName + " " : string.Empty) + x.Seller.LastName,
            //        Seller = $"{x.Seller.FirstName} {x.Seller.LastName}", //idiot e pravi testove v Judge i tova minawa, a ne gornoto
            //    })
            //    .AsEnumerable();

            ////with DTO:
            //var products = context.Products
            //    .Where(x => x.Price >= 500m && x.Price <= 1000m)
            //    .OrderBy(x => x.Price)  
            //    .Select(x => new ListProductsInRangeDTO
            //    {
            //        Name = x.Name,
            //        Price = x.Price,
            //        Seller = $"{x.Seller.FirstName} {x.Seller.LastName}", 
            //    })
            //    .AsEnumerable();

            //with Mapper:
            var products = context.Products
                .Where(x => x.Price >= 500m && x.Price <= 1000m)
                .OrderBy(x => x.Price)
                .ProjectTo<ListProductsInRangeDTO>()
                .AsEnumerable();

            var json = JsonConvert.SerializeObject(products, jsonSettings);

            //File.WriteAllText($"{outputPath}/products-in-range.json", json); //Judge gyrmi s towa.

            return json;
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            List<CategoryProduct> categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.AddRange(categoryProducts);

            var result = context.SaveChanges();

            return $"Successfully imported {result}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(inputJson)
                .Where(x => x.Name != null).ToList();

            context.Categories.AddRange(categories);

            var result = context.SaveChanges();

            return $"Successfully imported {result}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);

            var result = context.SaveChanges();

            return $"Successfully imported {result}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);

            var result = context.SaveChanges();

            return $"Successfully imported {result}";
        }

        private static string ResetDatabase(DbContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return "DB was created.";
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
        }

        private static void EnsureOutputDirectoryExists(string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
        }
    }
}