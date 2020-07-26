using System;

namespace Lecture_JSON_Processing
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    //using System.Text.Json;
    //using System.Text.Json.Serialization;
    using System.Threading;
    using System.Xml;
    using CsvHelper;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            //tova maha problemite s , i . pri razlichni cultures na razlichnite
            //pc-ta s razlichnite localni nachini za pisana na zapetajki i tochki i vremena i chasove, v razlichnite raioni na sveta.

            //WeatherForecast weather = new WeatherForecast();
            ////string jsonString = JsonSerializer.Serialize<WeatherForecast>(weather);
            ////pri templatenite classove, kogato se podrazbira templeitniq type na obekta, kojto podawa, toj moje da ne se 
            ////zapisva v <> i <> da se propusnat. eto taka:
            //string jsonString = JsonSerializer.Serialize(weather);
            //File.WriteAllText("../../../weather.json", jsonString);
            //Console.WriteLine(jsonString);

            //string file = "../../../weather.json";
            //string myJsonString = "{ \"Date\":\"2020-07-18T12:23:38.2374731Z\",\"TemperatureC\":30,\"Summary\":\"Hot summer day\"}";
            //string jsonString = File.ReadAllText(file);
            //WeatherForecast forecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString); //posochvam v kakyv type object da mi obyrne jsonstring-a!
            ////zashtoto pri deserializaciqta nqma kak da se znae kakyv e typa danni, s kojto rabotq i kojto iskam.
            //WeatherForecast forcastFromString = JsonSerializer.Deserialize<WeatherForecast>(myJsonString);
            //Console.WriteLine();

            ////po podoben nachin System.Text.Json pravi vzimaneto na dannite ot tipa na obekta i vijda kakvi sa imenata na propertytata mu i kakwo ima w tqh
            ////t.e. izpolzwa Reflection:
            //foreach (var property in typeof(WeatherForecast).GetProperties())
            //{
            //    Console.WriteLine(property.Name);
            //}
            //////vryshta:
            ////Date
            ////TemperatureC
            ////Summary

            //WeatherForecast weather = new WeatherForecast();
            //string jsonString = JsonSerializer.Serialize(weather, new JsonSerializerOptions
            //{
            //    WriteIndented = true, //taka mi se printva podreden json-a: na vseki red imam nov zapis, a ne vsichko na 1 red.
            //    IgnoreNullValues = true, //ako neshto e null, da ne mi go zapisva, t.e. ako imam property sys value null, ne mi go zapisvaj v json stringa.
            //    AllowTrailingCommas = true, //towa razreshava da ima zapetajka nakraq na json string, kojto shte se pravi na obekt.
            //    //ako towa e false i imam , nakraq na poslednoto property ot json stringa, to shte grymne deserializera!!!
            //});
            //File.WriteAllText("../../../weather.json", jsonString);
            //Console.WriteLine(jsonString);

            //WeatherForecast weather = new WeatherForecast();
            //string jsonString = JsonConvert.SerializeObject(weather, Formatting.Indented);
            //File.WriteAllText("../../../weather.json", jsonString);
            //Console.WriteLine(jsonString);

            //string file = "../../../weather.json";
            //var json = File.ReadAllText(file);
            //var weather = JsonConvert.DeserializeObject<WeatherForecast>(json);

            //string file = "../../../weather.json";
            //var weather = new WeatherForecast();
            //var json = JsonConvert.SerializeObject(weather, Formatting.Indented);
            //File.WriteAllText(file, json);
            //Console.WriteLine(json);
            //////vryshta towa:
            ////{
            ////      "Date": "2020-07-18T15:18:37.9743395Z",
            ////      "TemperatureC": 30,
            ////      "TemperaturesC": [
            ////        35,
            ////        25,
            ////        21,
            ////        33
            ////      ],
            ////      "Summary": "Hot summer day",
            ////      "Map": 3243335
            ////}

            //string file = "../../../weather.json";
            //var weather = new Forecast
            //{
            //    Prognoses = new List<WeatherForecast>
            //    {
            //        new WeatherForecast(),
            //        new WeatherForecast(),
            //        new WeatherForecast(),
            //    },
            //    AdditionalData = new Tuple<int, string>( 42, "Paulina" ),                
            //};
            //var json = JsonConvert.SerializeObject(weather, Formatting.Indented);
            //File.WriteAllText(file, json);
            //Console.WriteLine(json);

            //string file = "../../../weather.json";

            //var json = JsonConvert.SerializeObject(new WeatherForecast());
            //var anonimenNovTypeDanniObject = new
            //{
            //    TemperatureC = 0,
            //    Summary = string.Empty
            //};
            //var weather = JsonConvert.DeserializeAnonymousType(json, anonimenNovTypeDanniObject);

            //var weather = new WeatherForecast();
            //var json = JsonConvert.SerializeObject(weather, Formatting.Indented);
            //Console.WriteLine(json);

            //var weather = new WeatherForecast();
            //DefaultContractResolver contractResolver = new DefaultContractResolver()
            ////s tozi class moga da se vmykna v samiq algorithm za parse-vane i da vkaram nqkakwi nastrojki.
            ////tova e neshto kato nastrojkite v EF Core, koito pravq s modelBuilder-a.
            //{
            //    NamingStrategy = new SnakeCaseNamingStrategy() //"temperatures_c":
            //    //NamingStrategy = new KebabCaseNamingStrategy() //"temperatures-c":
            //    //NamingStrategy = new DefaultNamingStrategy() //"TemperaturesC":
            //    //NamingStrategy = new CamelCaseNamingStrategy() //"temperaturesC":
            //};
            //var jsonSettings = new JsonSerializerSettings //tova mi e classa s vsichki nastrojki.
            ////ako ne gi podam az otdelno, JSON.NET shte izpolzwa defaultnite si nastrojki.
            //{
            //    ContractResolver = contractResolver,
            //    Formatting = Formatting.Indented,
            //    Culture = CultureInfo.GetCultureInfo("bg-BG"), //shte polzwa , za desetichen razdelitel i t.n.
            //    //Culture = CultureInfo.InvariantCulture,
            //    //shte se polzwa towa pri cheteneto na , . dati, godini, chasove....
            //    //t.e. shte se polzwa UTC, shte se polzwa . za desetichen razdelitel
            //    //Converters --s tqh kazwam kak da se convertva ot edin type v drug, naprimer ot Array v Dictionary.
            //    //no defaultnite convertors vyrshat perfektna rabota.
            //    //kakva e Culture bi imalo znachenie samo pri deserializaciq, zashtoto pri serializaciq, json se pravi
            //    //s . za desetichen razdelitel i ne pita kakvo da e, za da nqma razlichni json-i generirani, a vsichki da sa
            //    //s ednakvi razdeliteli. Bi bilo typo da ostawqt vyzmojnost da se serializirat json-i s razlichni
            //    //izpolzwani cultures v tqh i towa da dowede do obyrkwaniq pri poluchatelite im, zashtoto poluchatelite nqma
            //    //da znaqt kakwa culture da izpolzwat, za da gi prochetat korektno.
            //    DateFormatString = "yyyy-MM-dd h:mm:ss tt",
            //    //NullValueHandling = NullValueHandling.Ignore, //igronira Null values ako ima zapisani
            //    NullValueHandling = NullValueHandling.Include, //vklyuhcva Null values ako ima zapisani
            //};
            //var json = JsonConvert.SerializeObject(weather, jsonSettings);
            //////moje i taka:
            ////var json = JsonConvert.SerializeObject(weather, new JsonSerializerSettings()
            ////{
            ////    ContractResolver = contractResolver,
            ////    Formatting = Formatting.Indented
            ////});
            //Console.WriteLine(json);
            //string file = "../../../weather.json";
            //File.WriteAllText(file, json);
            //var weatherDeserialised = JsonConvert.DeserializeObject<WeatherForecast>(File.ReadAllText(file), jsonSettings);

            //string file = "../../../weather.json";
            ////File.WriteAllText(file, JsonConvert.SerializeObject(new WeatherForecast(), Formatting.Indented));
            //var json = File.ReadAllText(file);
            //JObject jObject = JObject.Parse(json); //jObject e ot type JObject
            //foreach (var child in jObject.Children())
            //{
            //    Console.WriteLine(child); //child e vsqko edno property s name i value.
            //    foreach (var childOfChild in child)
            //    {
            //        Console.WriteLine(childOfChild);//tova sa samo values 
            //    }
            //}
            ////moga da izpolzwam JObject kato Dictionary:
            //Console.WriteLine(jObject["TemperatureC"]); //30
            //Console.WriteLine(jObject["TemperatureC"].Children()[0]); //Newtonsoft.Json.Linq.JEnumerable`1[Newtonsoft.Json.Linq.JToken]

            //string file = "../../../connectionStrings.json";
            //var json = File.ReadAllText(file);
            //JObject jObject = JObject.Parse(json); //jObject e ot type JObject i moga da izpolzwam LINQ v/u nego
            //var listOfStrings = jObject["connectionStrings"]
            //    .Where(x => x.ToString().Contains("Server")).ToList(); 
            ////trqbva da go stringosvam, za da go obrabotvam s 
            ////methodite na stringoobrabotkata!!!! Zashtoto x mi e ot type JToken i ne moga da polzwam Contains v/u nego!!!
            //foreach (var str in listOfStrings)
            //{
            //    Console.WriteLine(str);
            //}

            //            //tova e xml, v kojto imam spisyk ot person, kato person si ima Id, name i url propertyta:
            //            string xml = @"<?xml version='1.0' standalone='no'?> 
            // <root> 
            //    <person id='1'> 
            //        <name>Alan</name> 
            //        <url>www.google.com</url> 
            //    </person> 
            //    <person id='2'> 
            //        <name>Louis</name> 
            //        <url>www.yahoo.com</url> 
            //    </person> 
            //</root>";

            //            XmlDocument doc = new XmlDocument(); //XmlDocument e neshto kato xml parser, no pod formata na document.
            //            doc.LoadXml(xml); //doc stava parsenat xml obekt.
            //            string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented); //prevryshtam doc v json obekt.
            //            Console.WriteLine(jsonText);
            //            //vryshta mi:
            //            //          {
            //            //              "?xml": {
            //            //                  "@version": "1.0",
            //            //  "@standalone": "no"
            //            //              },
            //            //"root": {
            //            //                  "person": [
            //            //                    {
            //            //                      "@id": "1",
            //            //      "name": "Alan",
            //            //      "url": "www.google.com"
            //            //                    },
            //            //    {
            //            //                      "@id": "2",
            //            //      "name": "Louis",
            //            //      "url": "www.yahoo.com"
            //            //    }
            //            //  ]
            //            //}
            //            //          }

            ////cvs files to C# class objects:
            //string fileCsvToCSharp = "../../../carsCsvToCSharp.csv";
            //using (CsvReader reader = new CsvReader(new StreamReader(fileCsvToCSharp), CultureInfo.InvariantCulture))
            //{
            //    var carsToCSharp = reader.GetRecords<Car>();
            //    foreach (var car in carsToCSharp)
            //    {
            //        Console.WriteLine($"{car.Make} - {car.Model} - {car.Year}; {car.Price} lv.; {car.Description}");
            //    }
            //}
            ////tova mi izkara na Console:
            ////Ford - E350 - 1997; 3000.00 lv.; ac, abs, moon
            ////Chevy - Venture "Extended Edition" - 1999; 4900.00 lv.;
            ////Chevy - Venture "Extended Edition, Very Large" - 1999; 5000.00 lv.;
            ////Jeep - Grand Cherokee - 1996; 4799.00 lv.; MUST SELL! //tuk imam \n v Descriptiona na tozi ted!!! I towa mi e obraboteno prawilno!!!
            ////air, moon roof, loaded

            ////C# class objects to csv file:
            //string fileCSharpToCsv = "../../../carsCSharpToCsv.csv";
            //var carsToCsv = new List<Car>
            //{
            //    new Car{ Year = 2015, Make = "Hyundai", Model = "i20", Price = 36000m, Description = "My car"},
            //    new Car{ Year = 2011, Make = "Audi", Model = "Q5", Price = 96000m, Description = "Valery's car"},
            //};
            //using (CsvWriter writer = new CsvWriter(new StreamWriter(fileCSharpToCsv), CultureInfo.InvariantCulture))
            //{
            //    writer.WriteRecords(carsToCsv);
            //}
            ////towa imam zapisano vyv file:
            ////Year,Make,Model,Description,Price
            ////2015,Hyundai,i20,My car,36000
            ////2011,Audi,Q5,Valery's car,96000
            ///


            ////Kak se pravi byrzo deserialization ot JSON: copy-ram si sydyrjanieto na JSON faila v Clipboard-a,
            ////otivam v menuto Edit na VS i izbiram Paste Spacial -> Paste JSON as Classes!!! Pravi mi JSON Obekta na C# CLasses!!!
            ////ot towa copyrano neshto:
            //{
            //  "TemperatureC": 30,
            //  "TemperaturesC": [
            //    35,
            //    25,
            //    21,
            //    33
            //  ],
            //  "LongNameOfThisDecimalProperty": 0.0,
            //  "NullableProperty": null,
            //  "Summary2": null,
            //  "Map": 3243335,
            //  "dateOfTheForecast": "2020-07-18T19:55:54.8568531Z"
            //}
            ////stana towa:
            //public class Rootobject
            //{
            //    public int TemperatureC { get; set; }
            //    public int[] TemperaturesC { get; set; }
            //    public float LongNameOfThisDecimalProperty { get; set; }
            //    public object NullableProperty { get; set; }
            //    public object Summary2 { get; set; }
            //    public int Map { get; set; }
            //    public DateTime dateOfTheForecast { get; set; }
            //}

        }
    }
    class WeatherForecast
    {
        //public DateTime Date { get; set; } = DateTime.Now;
        [JsonProperty("dateOfTheForecast", Order = 4)] //tova shte e imeto na propertyto v json formata i shte e posledno po red.
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public int TemperatureC { get; set; } = 30;

        //public int[] TemperaturesC { get; set; } = new[] { 35, 25, 21, 33 };
        public List<int> TemperaturesC { get; set; } = new List<int> { 35, 25, 21, 33 };

        [JsonIgnore]
        public string Summary { get; set; } = "Hot summer day";

        [JsonProperty(Order = 1, Required = Required.Always)] //towa property, ako go nqma w json formata, pri deserializaciq
                                                              //shte izgyrmi
        public int Map { get; set; } = 3243335;

        public decimal LongNameOfThisDecimalProperty { get; set; }

        public string NullableProperty { get; set; }

        public string Summary2 { get; set; }


    }

    class Forecast
    {
        public List<WeatherForecast> Prognoses { get; set; } //tova se pravi na json objects array pri serializarion. 

        public Tuple<int, string> AdditionalData { get; set; } //tova se rpavi na obekt propertyta:
                                                               //"AdditionalData": {
                                                               //    "Item1": 42,
                                                               //    "Item2": "Paulina"
                                                               //  }
    }

    class Car
    {
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
