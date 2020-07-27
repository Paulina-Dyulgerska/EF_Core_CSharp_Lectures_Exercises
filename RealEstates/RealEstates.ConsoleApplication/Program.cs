using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RealEstates.ConsoleApplication
{
    class Program
    {
        static void Main( )
        {
           Console.OutputEncoding = Encoding.UTF8;

            var dbContext = new RealEstateContext();

            //tezi gi vikam samo pri moite testowe dali mi e OK nagradenata structura, posle gi maham i rabotq samo
            //s migrations!!!
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            dbContext.Database.Migrate();

            IRealEstatePropertiesService propertiesService = new RealEstatePropertiesService(dbContext);

            //propertiesService.Create("Стрелбище", 190, 2001, 250000, "5-стаен", "Тухла", 4, 9);
            //propertiesService.Create("Дианабад", 190, 2011, 350000, "6-стаен", "ЕПК", 6, 9);
            //propertiesService.Create("Гагарин", 92, 2005, 67000, "4-стаен", "Тухла", 4, 4);
            //propertiesService.Create("Иван Вазов", 52, 2015, 67000, "2-стаен", "Панел", 4, 8);
            //propertiesService.Create("Иван Вазов", 152, 2015, 157000, "3-стаен", "Тухла", 2, 8);
            //propertiesService.Create("Гео Милев", 152, 2015,456000, "4-стаен", "Тухла", 3, 6);
            //propertiesService.Create("Достоевски", 67, 1898,45600, "4-стаен", "Тухла", 3, 6);

            IDistrictService districtService = new DistrictService(dbContext);
            
            //var districts = districtService.GetTopDistrictsByAveragePrice();
            
            //foreach (var district in districts)
            //{
            //    Console.WriteLine($"{district.Name} => Price: {district.minPrice} - {district.maxPrice}; " +
            //        $"AveragePrice: {district.AveragePrice}; Count: {district.RealEstatePropertiesCount}");
            //};

            var districtsByNumberOfProperties = districtService.GetTopDistrictsByNumberOfProperties();

            foreach (var district in districtsByNumberOfProperties)
            {
                Console.WriteLine($"{district.Name} => Price: {district.minPrice} - {district.maxPrice}; " +
                    $"AveragePrice: {district.AveragePrice:0.00}; Count: {district.RealEstatePropertiesCount}");
            };


        }
    }
}
