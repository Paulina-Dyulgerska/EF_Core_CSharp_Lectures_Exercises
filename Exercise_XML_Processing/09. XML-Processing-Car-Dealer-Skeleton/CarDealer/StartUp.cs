using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.XMLHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        private const string outputPath = "../../../Results";

        private const string datasetsPath = "../../../Datasets";

        private static IMapper mapper;

        public static void Main()
        {
            //EnsureOutputDirectoryExists(outputPath);

            //mapper = InitializeMapper(); //v methodite, v koito go polzwa, go instanciram tam, zashtoto Judge ne
            ////vika Main() i ne moga da go polzwam ot tuk!!!!

            var dbContext = new CarDealerContext();

            //var createdDB = ResetDatabase(dbContext);
            //Console.WriteLine(createdDB);

            //ImportSuppliers(dbContext, File.ReadAllText($"{datasetsPath}/suppliers.xml"));
            //ImportParts(dbContext, File.ReadAllText($"{datasetsPath}/parts.xml"));
            //ImportCars(dbContext, File.ReadAllText($"{datasetsPath}/cars.xml"));
            //ImportCustomers(dbContext, File.ReadAllText($"{datasetsPath}/customers.xml"));
            //ImportSales(dbContext, File.ReadAllText($"{datasetsPath}/sales.xml"));

            var result = GetSalesWithAppliedDiscount(dbContext);

            Console.WriteLine(result);
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
             .Select(s => new SaleExportDTO
             {
                 Car = new CarExportDTO
                 {
                     Make = s.Car.Make,
                     Model = s.Car.Model,
                     TravelledDistance = s.Car.TravelledDistance,
                 },
                 Discount = s.Discount,
                 CustomerName = s.Customer.Name,
                 Price = s.Car.PartCars.Select(x => x.Part.Price).Sum(),
                 PriceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100m,
             })
             .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(SaleExportDTO[]), new XmlRootAttribute("sales"));
            var xml = new StringBuilder();
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            //moej i taka da se slojat namespaces da sa empty:
            //XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new[]
            //{
            //    XmlQualifiedName.Empty,
            //});
            serializer.Serialize(new StringWriter(xml), sales, namespaces);

            return xml.ToString();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            ////Kogato EF ne raboti tuk zaradi 2-te aggregirashti functions edna sled druga, pravq Grouping izvyn DB-a!!!
            ////tova ne raboti tuk zaradi 2-te aggregirashti functions edna sled druga,
            ////a raboteshe v JSON Exercises!!! Zatowa pravq grupinga po-dolu.
            //var customers = context.Customers
            //    .Where(c => c.Sales.Count > 0)
            //    .Select(c => new CustomerSpentMoneyDTO
            //    {
            //        FullName = c.Name,
            //        BoughtCars = c.Sales.Count,
            //        SpentMoney = c.Sales.Select(s => s.Car.PartCars.Select(pc => pc.Part.Price).Sum()).AsEnumerable().Sum(),
            //    })
            //    .OrderByDescending(c => c.SpentMoney)
            //    .ToArray();

            ////towa e proverka za kupenite koli ot customer s id = 1:
            //var d = context.Cars.Where(c => c.Id == 3 || c.Id == 4 || c.Id == 34)
            //        .Select(c => c.PartCars.Select(p => p.Part.Price).Sum())
            //        .ToList().Sum();
            //Console.WriteLine("d = " + d);

            //Pravq Grouping:
            //vzimam vsqka edna prodajba i kolko struva kolata v neq i poluchawam Collectiona sales:
            var sales = context.Sales
                .Select(c => new CustomerSpentMoneyDTO
                {
                    FullName = c.Customer.Name,
                    BoughtCars = c.Customer.Sales.Count,
                    SpentMoney = c.Car.PartCars.Select(pc => pc.Part.Price).Sum(),
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            //posle grupiram po imeto na customera i 
            //taka vsqka group ima Key = FullName, a zad tozi Key stoi Collection ot vsichki pokupki s kupenite
            //koli ot uzera kato vsqka kola e sys smetnata cena. Vzimam Collectiona i si pravq Sum() na cenite na
            //vsichki koli v nego - taka imam obshtata suma na parite dadeni za koli ot wseki customer.
            //Poneje groupata e naprawena po customer-a, to v COllectiona kojto stoi zad wseki Key, imam
            //danni za BoughtCars i SpentMoney i FullName, zatowa kojto ot zapisite v Collection da vzema,
            //ot vseki shte moga da si vzema BoughtCars brojkata i FullName i te shte sa edni i syshti za wseki zapis
            //v Collectiona, kojto Collection prinadleji na Vsqka edna group
            var customers = sales.GroupBy(x => x.FullName)
                 .Select(g => new CustomerSpentMoneyDTO
                 {
                     FullName = g.FirstOrDefault().FullName, //vseki zapis ima FullName.
                     BoughtCars = g.FirstOrDefault().BoughtCars, //vseki zapis ima BoughtCars
                     SpentMoney = g.Sum(x => x.SpentMoney), //aggregiram Vsichki sumi ot vsichki zapisi ot grupata
                 })
                 .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(CustomerSpentMoneyDTO[]), new XmlRootAttribute("customers"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            var xml = new StringBuilder();
            serializer.Serialize(new StringWriter(xml), customers, namespaces);

            return xml.ToString();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new CarWithPartsExportDTO
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    PartCars = c.PartCars.Select(pc => new PartForCarWithPartsExportDTO
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price,
                    })
                    .OrderByDescending(p => p.Price)
                    .ToList(),
                })
             .OrderByDescending(c => c.TravelledDistance)
             .ThenBy(c => c.Model)
             .Take(5)
             .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(CarWithPartsExportDTO[]), new XmlRootAttribute("cars"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            var xml = new StringBuilder();
            serializer.Serialize(new StringWriter(xml), cars, namespaces);

            return xml.ToString();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var mapper = InitializeMapper(); //instanciram go tuk, zashtoto Judge ne go vika ot Main() inache!!!

            var suppliers = context.Suppliers
                .Where(s => s.IsImporter != true)
                .ProjectTo<SupplierExportDTO>(mapper.ConfigurationProvider)
                .ToList();

            ////bez mapper se pravi taka:
            //var suppliers = context.Suppliers
            //    .Where(s => !s.IsImporter)
            //    .Select(x => new SupplierExportDTO
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        PartsCount = x.Parts.Count
            //    })
            //    .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(List<SupplierExportDTO>), new XmlRootAttribute("suppliers"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            var xml = new StringBuilder();
            serializer.Serialize(new StringWriter(xml), suppliers, namespaces);

            return xml.ToString();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var mapper = InitializeMapper(); //instanciram go tuk, zashtoto Judge ne go vika ot Main() inache!!!

            var cars = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ProjectTo<CarExportWithAttributesDTO>(mapper.ConfigurationProvider)
                .ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CarExportWithAttributesDTO>), new XmlRootAttribute("cars"));
            var xml = new StringBuilder();
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(new StringWriter(xml), cars, namespaces);

            return xml.ToString();
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2_000_000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
            var xml = new StringBuilder();
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(new StringWriter(xml), cars, namespaces);

            return xml.ToString();
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var carIds = context.Cars.Select(c => c.Id).ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<Sale>), new XmlRootAttribute("Sales"));
            var sales = ((List<Sale>)serializer.Deserialize(new StringReader(inputXml)))
                .Where(s => carIds.Any(i => i == s.CarId))
                .ToList();

            context.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Customer>), new XmlRootAttribute("Customers"));
            var customers = ((List<Customer>)serializer.Deserialize(new StringReader(inputXml))).ToList();

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CarImportDTO>), new XmlRootAttribute("Cars"));
            var cars = ((List<CarImportDTO>)serializer.Deserialize(new StringReader(inputXml))).ToList();

            foreach (var car in cars)
            {
                var currentCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance,
                };

                foreach (var partId in car.PartCars.Select(pc => pc.PartId).Distinct().ToList())
                {
                    var partCar = new PartCar
                    {
                        PartId = partId,
                        Car = currentCar,
                    };

                    currentCar.PartCars.Add(partCar);
                    context.PartCars.Add(partCar);
                }
                context.Cars.Add(currentCar);
            }

            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var supplierIds = context.Suppliers.Select(s => s.Id).ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<Part>), new XmlRootAttribute("Parts"));
            var parts = ((List<Part>)serializer.Deserialize(new StringReader(inputXml)))
                .Where(p => supplierIds.Any(e => e == p.SupplierId)).ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Supplier>), new XmlRootAttribute("Suppliers"));
            var suppliers = (List<Supplier>)serializer.Deserialize(new StringReader(inputXml));

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        private static string ResetDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return "DB was created.";
        }

        private static void EnsureOutputDirectoryExists(string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
        }

        private static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cnf =>
            {
                cnf.AddProfile(new CarDealerProfile());
                //cnf.AddProfile<CarDealerProfile>();
            });

            return config.CreateMapper();
        }
    }
}