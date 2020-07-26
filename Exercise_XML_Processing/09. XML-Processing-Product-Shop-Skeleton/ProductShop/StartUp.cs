using AutoMapper;
using AutoMapper.QueryableExtensions;
using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Models;
using ProductShop.XMLHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        private const string outputPath = "../../../Results";

        private const string datasetsPath = "../../../Datasets";

        private static IMapper mapper;

        public static void Main()
        {
            EnsureOutputDirectoryExists(outputPath);

            using var dbContext = new ProductShopContext();

            ResetDatabase(dbContext);

            InitializeMapper();

            //var usersInputXml = File.ReadAllText("../../../Datasets/users.xml");
            //ImportUsers(dbContext, usersInputXml);

            //var productsInputXml = File.ReadAllText("../../../Datasets/products.xml");
            //ImportProducts(dbContext, productsInputXml);

            //var categoriesInputXml = File.ReadAllText("../../../Datasets/categories.xml");
            //ImportCategories(dbContext, categoriesInputXml);

            //var categoriesProductsInputXml = File.ReadAllText("../../../Datasets/categories-products.xml");
            //ImportCategoryProducts(dbContext, categoriesProductsInputXml);

            var result = GetUsersWithProducts(dbContext);

            Console.WriteLine(result);

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            List<UserWithSoldProductsWithCountDTO> users = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .ToList() //tazi glupost tuk q pravq zaradi Judge, zashtoto bez neq ne minawa!!!!
                .Select(u => new UserWithSoldProductsWithCountDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsWithCountDTO
                    {
                        Products = new SoldProductsDTO
                        {
                            Products = u.ProductsSold
                            .Select(sp => new ProductInExportUserWithSoldProductsDTO
                            {
                                Name = sp.Name,
                                Price = sp.Price,
                            })
                            .OrderByDescending(sp => sp.Price)
                            .ToList(),
                        },
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToList();

            var countUsers = users.Count;

            AllUsersDTO allUsers = new AllUsersDTO
            {
                Count = countUsers,
                Users = new AllusersListDTO
                {
                    Users = users.Take(10).ToList(),
                }
            };

            XmlSerializer serializer = new XmlSerializer(typeof(AllUsersDTO), new XmlRootAttribute("Users"));
            var xml = new StringBuilder();
            var writer = new StringWriter(xml);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(writer, allUsers, namespaces);

            return xml.ToString();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new CategoryWIthProductCountRevenueAveragePriceDTO
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price),
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(CategoryWIthProductCountRevenueAveragePriceDTO[]),
                new XmlRootAttribute("Categories"));
            var xml = new StringBuilder();
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            using var writer = new StringWriter(xml);
            serializer.Serialize(writer, categories, namespaces);

            return xml.ToString();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Count > 0)
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new UserWithSoldProductsDTO
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = new SoldProductsDTO
                    {
                        Products = x.ProductsSold.Select(p => new ProductInExportUserWithSoldProductsDTO
                        {
                            Name = p.Name,
                            Price = p.Price,
                        }).ToList(),
                    }
                })
                .Take(5)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(UserWithSoldProductsDTO[]),
                new XmlRootAttribute("Users"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            var xml = new StringBuilder();
            using var strWriter = new StringWriter(xml);
            serializer.Serialize(strWriter, users, namespaces);

            return xml.ToString();
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ProductDTO>), new XmlRootAttribute("Products"));

            var xml = new StringBuilder();

            using var writer = new StringWriter(xml);

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(x => new ProductDTO
                {
                    Name = x.Name,
                    Price = x.Price,
                    BuyerFullName = x.Buyer.FirstName + " " + x.Buyer.LastName
                })
                .OrderBy(p => p.Price)
                .Take(10)
                .ToList();
            //.ProjectTo<ExportProductDTO>(mapper.ConfigurationProvider).ToList(); // Judge ne haresva towa

            //defaultnoto povedenie e da mi sloji namespaces ako napisha tova:
            //serializer.Serialize(writer, products, xmlNamespaces);
            //< Products xmlns: xsi = "http://www.w3.org/2001/XMLSchema-instance" xmlns: xsd = "http://www.w3.org/2001/XMLSchema" >
            //te obache mi habqt memory i Judge mi dawa Out of memory limit.
            //zatowa gi pravq da sa null i taka minawam v Judge!!!!

            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(writer, products, xmlNamespaces);

            return xml.ToString();
        }

        //public static string GetProductsInRange(ProductShopContext context)
        //{
        //    const string rootName = "Products";
        //    var targetProducts = context.Products
        //        .Where(x => x.Price >= 500 && x.Price <= 1000)
        //        .Select(x => new ExportProductDTO
        //        {
        //            Name = x.Name,
        //            Price = x.Price,
        //            BuyerFullName = x.Buyer.FirstName + " " + x.Buyer.LastName
        //        })
        //        .OrderBy(x => x.Price)
        //        .Take(10)
        //        .ToArray();
        //    var resultXml = XmlConverter.Serialize(targetProducts, rootName);
        //    return resultXml;
        //}

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CategoryProduct>), new XmlRootAttribute("CategoryProducts"));

            var categories = context.Categories.Select(x => x.Id).ToList();
            var products = context.Products.Select(x => x.Id).ToList();
            var categoryProducts = ((List<CategoryProduct>)serializer.Deserialize(new StringReader(inputXml)))
                .Where(cp => (categories.Any(x => x == cp.CategoryId)) && (products.Any(x => x == cp.ProductId))).ToList();

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Category>), new XmlRootAttribute("Categories"));
            var categories = (List<Category>)serializer.Deserialize(new StringReader(inputXml));

            context.Categories.AddRange(categories.Where(x => x.Name != null).ToList());

            var result = context.SaveChanges();

            return $"Successfully imported {result}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Product>), new XmlRootAttribute("Products"));
            var products = (List<Product>)serializer.Deserialize(new StringReader(inputXml));

            //tova ne raboti v judge:
            //var products = (List<Product>)serializer.Deserialize(File.OpenRead("../../../Datasets/products.Xml"));

            context.Products.AddRange(products);

            var result = context.SaveChanges();

            return $"Successfully imported {result}";
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>), new XmlRootAttribute("Users"));
            //var users = (List<User>)serializer.Deserialize(File.OpenRead("../../../Datasets/users.Xml"));
            var users = (List<User>)serializer.Deserialize(new StringReader(inputXml));

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
            var config = new MapperConfiguration(cnf =>
            {
                cnf.AddProfile(new ProductShopProfile());
                //cnf.AddProfile<ProductShopProfile>();
            });

            mapper = config.CreateMapper();
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