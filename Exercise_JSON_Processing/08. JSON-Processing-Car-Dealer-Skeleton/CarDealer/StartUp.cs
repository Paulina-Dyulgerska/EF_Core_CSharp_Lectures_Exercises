using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CarDealer
{
    public class StartUp
    {
        private const string outputPath = "../../../Results";
        private const string jsonDatasetsPath = "../../../Datasets";
        public static void Main()
        {
            var dbContext = new CarDealerContext();

            ResetDatabase(dbContext);

            InitializeMapper();

            EnsureOutputDirectoryExists(outputPath);

            //ImportSuppliers(dbContext, File.ReadAllText($"{jsonDatasetsPath}/suppliers.json"));
            //ImportParts(dbContext, File.ReadAllText($"{jsonDatasetsPath}/parts.json"));
            //ImportCars(dbContext, File.ReadAllText($"{jsonDatasetsPath}/cars.json"));
            //ImportCustomers(dbContext, File.ReadAllText($"{jsonDatasetsPath}/customers.json"));
            //ImportSales(dbContext, File.ReadAllText($"{jsonDatasetsPath}/sales.json"));

            var result = GetTotalSalesByCustomer(dbContext);

            Console.WriteLine(result);

        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                //ContractResolver = new DefaultContractResolver
                //{
                //    NamingStrategy = new CamelCaseNamingStrategy(),
                //},
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Include,
            };

            var sales = context.Sales
                .Select(s => new
                {
                    Car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TravelledDistance,
                    },
                    CustomerName = s.Customer.Name,
                    s.Discount,
                    Price = s.Car.PartCars.Select(pc => pc.Part).Select(p => p.Price).Sum(),
                })
                .Select(s => new
                {
                    car = s.Car,
                    customerName = s.CustomerName,
                    Discount = s.Discount.ToString("f2"),
                    price = s.Price.ToString("f2"),
                    priceWithDiscount = ((1 - s.Discount/100) * s.Price).ToString("f2"),
                })
                .Take(10) //kydeto i da go sloja tozi Take e vse taq, zashtoto edva v AsEnumerable() shte mi se napravi
                //zaqwkata i tq shte e samo 1 i shte vklyuchi Take(), ako shte da e pyrvi ili posleden v C# coda!!!
                .AsEnumerable();

            var json = JsonConvert.SerializeObject(sales, jsonSettings);

            //File.WriteAllText($"{outputPath}/sales-discounts.json", json);

            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Include,
            };

            var customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Select(s => s.Car.PartCars.Select(pc => pc.Part.Price).Sum()).Sum(),
                })
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .AsEnumerable();

            var json = JsonConvert.SerializeObject(customers, jsonSettings);

            //File.WriteAllText($"{outputPath}/customers-total-sales.json", json);

            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new DefaultNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Include,
            };

            var cars = context.Cars
                .Select(x => new
                {
                    car = new
                    {
                        x.Make,
                        x.Model,
                        x.TravelledDistance,
                    },
                    parts = x.PartCars.Select(p => new { p.Part.Name, Price = p.Part.Price.ToString("f2") }),
                })
                .AsEnumerable();

            var json = JsonConvert.SerializeObject(cars, jsonSettings);

            //File.WriteAllText($"{outputPath}/cars-and-parts.json", json);

            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new DefaultNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Include,
            };

            var suppliers = context.Suppliers
                .Where(x => x.IsImporter != true)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    PartsCount = x.Parts.Count,
                })
                .AsEnumerable();

            var json = JsonConvert.SerializeObject(suppliers, jsonSettings);

            //File.WriteAllText($"{outputPath}/local-suppliers.json", json);

            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new DefaultNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Include,
            };

            var cars = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .Select(x => new
                {
                    x.Id,
                    x.Make,
                    x.Model,
                    x.TravelledDistance,
                })
                .AsEnumerable();

            var json = JsonConvert.SerializeObject(cars, jsonSettings);

            //File.WriteAllText($"{outputPath}/toyota-cars.json", json); //Judge gyrmi s towa.

            return json;
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new DefaultNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                Culture = CultureInfo.InvariantCulture,
                NullValueHandling = NullValueHandling.Include,
            };

            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                    x.IsYoungDriver,
                })
                .AsEnumerable();

            var json = JsonConvert.SerializeObject(customers, jsonSettings);

            File.WriteAllText($"{outputPath}/ordered-customers.json", json); //Judge gyrmi s towa.

            return json;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);

            var result = context.SaveChanges();

            return $"Successfully imported {result}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.Customers.AddRange(customers);

            var result = context.SaveChanges();

            return $"Successfully imported {result}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carDTOs = JsonConvert.DeserializeObject<List<CarDTO>>(inputJson);


            foreach (var carDTO in carDTOs)
            {

                var car = new Car
                {
                    Make = carDTO.Make,
                    Model = carDTO.Model,
                    TravelledDistance = carDTO.TravelledDistance,
                };

                foreach (var part in carDTO.PartsId.Distinct()) //imam povtarqshti se id-ta na parts v spisyka.
                {
                    var partCar = new PartCar
                    {
                        PartId = part,
                        Car = car,
                    };

                    car.PartCars.Add(partCar);
                    context.PartCars.Add(partCar);
                }

                context.Cars.Add(car);
            }

            context.SaveChanges();

            return $"Successfully imported {carDTOs.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            foreach (var part in parts)
            {
                if (context.Suppliers.Any(x => x.Id == part.SupplierId))
                {
                    context.Parts.Add(part);
                }
            }

            var result = context.SaveChanges();

            return $"Successfully imported {result}.";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);

            var result = context.SaveChanges();

            return $"Successfully imported {result}.";
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
                cfg.AddProfile<CarDealerProfile>();
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