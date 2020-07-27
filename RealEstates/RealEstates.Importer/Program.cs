using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using RealEstates.Data;
using RealEstates.Services;
using System.Linq;

namespace RealEstates.Importer
{
    class Program
    {
        static void Main()
        {
            var json = File.ReadAllText("imot.bg-raw-data-2020-07-23.json");

            var realEstateProperties = JsonSerializer.Deserialize<IEnumerable<JsonProperty>>(json);

            var dbContext = new RealEstateContext();

            IRealEstatePropertiesService realEstatePropertiesService = new RealEstatePropertiesService(dbContext);
            foreach (var res in realEstateProperties.Where(x => x.Price > 1000))
            //za da mahna tezi s nerealistichnite ceni, vzimam samo tezi > 1000.
            {
                try
                {
                    realEstatePropertiesService.Create(
                        res.District,
                        res.Size,
                        res.Year,
                        res.Price,
                        res.Type,
                        res.BuildingType,
                        res.Floor,
                        res.TotalFloors);
                }
                catch (Exception)
                {

                    //throw; //az ne znam kakvo shte pravq s tazi error i prodyljavam natatyk, kato prosto nqma
                    //da zapisha nishto v DB-a. No moqt Service throwna error i toj se hvana tuk - ot klienta na 
                    //moq Service, no kojto shte polzwa tozi cod, shte reshi kakwo da pravi s error-a. Az samo mu davam 
                    //info za towa kakwo ne e nared, towa mu prashta Service na tozi, kojto go polzwa. Towa da pravi Service
                    //error na polzwashtiqt go, e pravilnoto povedenie na Service-to!!!
                }
            }
        }
    }
}
