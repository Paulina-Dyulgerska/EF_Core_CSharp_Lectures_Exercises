using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Lecture_XML_Processing
{
    class Program
    {
        static void Main(string[] args)
        {
            //To process an XML string:
            ////kak se pravi XML - deserialization:

            //var xml = File.ReadAllText("Planes.xml");
            //XDocument xmlDoc = XDocument.Parse(xml); //parsevam string
            //XDocument xmlDocument = XDocument.Load("Planes.xml"); //parsevam file
            //Console.WriteLine(XDocument.Load("Library.xml").Root
            //    .Elements()
            //    .FirstOrDefault(x => x.Element("BookName").Value == "It")); //tova ne raboti
            //Console.WriteLine(xmlDocument.Declaration.Encoding); //utf-8
            //Console.WriteLine(xmlDocument.Declaration.Version); //1.0
            //Console.WriteLine(xmlDocument.Root.Elements().Count()); //2
            //Console.WriteLine(xmlDocument.Root.Elements().FirstOrDefault(x => x.Element("color").Value.Contains("Blue"))
            //    .Element("year").Value);



            ////kak se pravi XML - deserialization:

            //Console.OutputEncoding = Encoding.UTF8;
            //var xmlDocument = XDocument.Load("../../../bgwiki-20200701-abstract.xml"); //parsevam file
            //foreach (var article in xmlDocument.Root.Elements())
            //{
            //    //article.SetAttributeValue("lang", "bg"); //promenqm attributes na vseki child element ot root-a
            //    ////t.e. na vseki <doc> mu setnah attribute!
            //    //article.Element("title").SetAttributeValue("lang", "bg"); //shte sloji na <title> attribute lang="bg"
            //    ////ako setna null na daden attribute, toj prosto shte byde iztrit:
            //    //article.SetAttributeValue("lang", null); //triq attribute lang ot <doc>
            //    //article.Element("title").SetAttributeValue("lang", null); //triq attribute lang ot <title>
            //    //po syshtiqt nachin raboti i za elementi:
            //    //article.SetElementValue("links", null); //iztriwam celiqt element <links> s vsichko v nego!!!
            //    article.SetElementValue("links", "nqmame"); //dobavih nov element (tag) <links>nqmame</links> !!!!
            //}
            //xmlDocument.Save("../../../bgwiki_updated.xml"); //kogato sym gotova s promenite v/u xml-a, pravq Save().



            ////kak se pravi XML - deserialization:

            //Console.OutputEncoding = Encoding.UTF8;
            //var xmlDocument = XDocument.Load("../../../bgwiki_updated.xml"); //parsevam file
            //var articles = xmlDocument.Root
            //    .Elements() //towa sa <doc> tags.
            //    .Select(x => new
            //    {
            //        Title = x.Element("title").Value,
            //        Description = x.Element("abstract").Value,
            //        Url = x.Element("url").Value,
            //    })
            //    .Where(x => x.Title.Contains("Паулина"))
            //    .OrderBy(x => x.Title);

            ////var articles = xmlDocument.Root
            ////    .Elements() //towa sa <doc> tags.
            ////    .Where(x => x.Element("title").Value.Contains("Паулина"))
            ////    .OrderBy(x => x.Element("title").Value)
            ////    .Select(x => new
            ////    {
            ////        Title = x.Element("title").Value,
            ////        Description = x.Element("abstract").Value,
            ////        Url = x.Element("url").Value,
            ////    });

            //foreach (var article in articles)
            //{
            //    Console.WriteLine(article.Title);
            //}



            ////kak se pravi XML - serialization:

            //XDocument xmlDocument = new XDocument(new XElement("books"));
            //for (int i = 0; i < 100; i++)
            //{
            //    var bookElement = new XElement("book");
            //    bookElement.SetAttributeValue("lang", "bg");
            //    bookElement.Add(new XElement("year", 1876));
            //    bookElement.Add(new XElement("title", "Tom Soyer"));
            //    bookElement.Add(new XElement("author", "Mark Twain"));
            //    xmlDocument.Root.Add(bookElement);
            //}
            ////xmlDocument.Save("../../../myBooks.xml", SaveOptions.DisableFormatting);
            //xmlDocument.Save("../../../myBooks.xml");



            //////kak se pravi XML - deserialization:
            ////kak da si vzimam oshte po-elegantno nesthata ot xml-a bez da opisvam neshta kato towa: Description = x.Element("abstract").Value,

            //Console.OutputEncoding = Encoding.UTF8;
            ////ako property ne e public, to ne se serializira!!!
            ////ako class ne e public, toj ne se serializira!!!
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(doc[]), new XmlRootAttribute("feed")); //pravq si serializer
            ////kojto ima Root Element s ime "feed"!!! Taka shte mu kaja kyde da hodi da chete ot xml faila.
            //var docs = (doc[])xmlSerializer.Deserialize(File.OpenRead("../../../bgwiki_updated.xml"));
            ////otvarqm file stream da go chete serializera, a prochetenoto e ot type object, zatowa trqbwa da si go
            ////parse-na az do type doc!!!!
            //var articles = docs.Where(x => x.title.Contains("Паулина")).OrderBy(x => x.title); //vzimam kakvoto mi trqbwa ot docs[]

            //foreach (var article in articles)
            //{
            //    Console.WriteLine(article.title);
            //}



            //////kak se pravi XML - serialization:

            //List<doc> docs = new List<doc>
            //{
            //    new doc{title = "България - Паулина", @abstract = "Държава....", url = "https://wikipedia.bg"},
            //    new doc{title = "България - Паулина и Иван живеят в нея", @abstract = "Държавата ни....", url = "https://wikipedia.bg"}
            //};

            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<doc>), new XmlRootAttribute("feed"));
            ////da vnimavam: ako pyrvo imeto na feed mi e bilo defaultnoto, koeto e ArrayOfDoc, i posle go smenq na feed i pusna
            ////na novo serializaciqta, to v xml faila ne mi se rename-va pravilno i ostawam chasti ot staroto ime, koito chupqt
            ////faila. Izvod - kakyv Root Element si napisha, takyv da si go polzwam i da ne go promenqm v dvijenie!!!!
            //xmlSerializer.Serialize(File.OpenWrite("../../../myWiki.xml"), docs);

            ////syzdade mi towa:
            ////<? xml version = "1.0" ?>
            ////<feed xmlns:xsi = "http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd ="http://www.w3.org/2001/XMLSchema">
            ////      <doc>
            ////        <title>България - Паулина</title>
            ////        <abstract>Държава....</abstract>
            ////        <url>https://wikipedia.bg</url>
            ////      </doc>
            ////      <doc>
            ////        <title>България - Паулина и Иван живеят в нея</title>
            ////        <abstract>Държавата ни....</abstract>
            ////        <url>https://wikipedia.bg</url>
            ////      </doc>
            ////</feed>



            //////kak se pravi XML - serialization with attributes:

            //List<Article> docs = new List<Article>
            //{
            //    new Article{Title = "България - Паулина", Description = "Държава...." },
            //    new Article{Title = "България - Паулина и Иван живеят в нея", Description = "Държавата ни...."},
            //};

            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Article>), new XmlRootAttribute("feed"));
            ////da vnimavam: ako pyrvo imeto na feed mi e bilo defaultnoto, koeto e ArrayOfDoc, i posle go smenq na feed i pusna
            ////na novo serializaciqta, to v xml faila ne mi se rename-va pravilno i ostawam chasti ot staroto ime, koito chupqt
            ////faila. Izvod - kakyv Root Element si napisha, takyv da si go polzwam i da ne go promenqm v dvijenie!!!!
            //xmlSerializer.Serialize(File.OpenWrite("../../../myWiki.xml"), docs);

            ////syzdade mi towa:
            ////<? xml version = "1.0" ?>
            ////<feed xmlns:xsi = "http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd ="http://www.w3.org/2001/XMLSchema">
            ////      <doc>
            ////        <title>България - Паулина</title>
            ////        <abstract>Държава....</abstract>
            ////      </doc>
            ////      <doc>
            ////        <title>България - Паулина и Иван живеят в нея</title>
            ////        <abstract>Държавата ни....</abstract>
            ////      </doc>
            ////</feed>



            //////kak se pravi XML - deserialization:

            //Console.OutputEncoding = Encoding.UTF8;
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Article>), new XmlRootAttribute("feed"));
            //var docs = (List<Article>)xmlSerializer.Deserialize(File.OpenRead("../../../bgwiki_updated.xml"));
            //var articles = docs.Where(x => x.Title.Contains("Паулина")).OrderBy(x => x.Title); 
            //foreach (var article in articles)
            //{
            //    Console.WriteLine(article.Title);
            //    Console.WriteLine("   " + article.Description);
            //    Console.WriteLine(new String('-', 60));
            //}


            ////Kak se pravi byrzo deserialization ot XML: copy-ram si sydyrjanieto na XML faila v Clipboard-a,
            ////otivam v menuto Edit na VS i izbiram Paste Spacial -> Paste XML as Classes!!! Pravi mi XML Obekta na C# CLasses!!!
            ////ot towa copyrano neshto:
            //< book lang = "bg" >
            //   < year > 1876 </ year >
            //   < title > Tom Soyer </ title >
            //   < author > Mark Twain </ author >
            //</ book >
            ////mi napravi towa:
            // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
            //[System.SerializableAttribute()]
            //[System.ComponentModel.DesignerCategoryAttribute("code")]
            //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
            //[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
            //public partial class feed
            //{
            //    [System.Xml.Serialization.XmlElementAttribute("doc")]
            //    public feedDoc[] doc { get; set; }
            //}
            //[System.SerializableAttribute()]
            //[System.ComponentModel.DesignerCategoryAttribute("code")]
            //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
            //public partial class feedDoc
            //{
            //    public string title { get; set; }
            //    public string @abstract { get; set; }
            //}


            //-------------------
            //////kak se pravi XML - deserialization:

            Console.OutputEncoding = Encoding.UTF8;
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Article>), new XmlRootAttribute("planes"));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Article>), new XmlRootAttribute("feed"));
            //var docs = (List<Article>)xmlSerializer.Deserialize(File.OpenRead("../../../planes.xml"));
            var docs = (List<Article>)xmlSerializer.Deserialize(File.OpenRead("../../../bgwiki_updated.xml"));
            var articles = docs.Where(x => x.Title.Contains("Паулина")).OrderBy(x => x.Title).ToList(); 
            foreach (var article in articles)
            {
                Console.WriteLine(article.Title);
                Console.WriteLine("   " + article.Description);
                Console.WriteLine(new String('-', 60));
            }

            xmlSerializer.Serialize(File.OpenWrite("test.xml"), articles); //podreden file
            using (var xmlWriter = XmlWriter.Create(File.OpenWrite("test_unintended.xml"), new XmlWriterSettings { Indent = false })) //nepodreden file
            {
                xmlSerializer.Serialize(xmlWriter, articles); //taka si pravq nepodreden xml file, t.e. Unindented formatting.
            }
            //instariram si System.Text.Json libraryto.
            File.WriteAllText("test.json", JsonSerializer.Serialize(articles, new JsonSerializerOptions { WriteIndented = true })); //podreden file
            File.WriteAllText("test_unintended.json", JsonSerializer.Serialize(articles, new JsonSerializerOptions { WriteIndented = false })); //nepodreden file

            //taka se pravi Binary Serialization!!!! No da ne zabrawq da sloja attribute [Serializible] na towa, koeto iskam da
            //serializiram, t.e. nad class Article v sluchaq!!!    [Serializable] nad    public class Article se slaga!!!!
            var binarySerialiser = new BinaryFormatter();
            binarySerialiser.Serialize(File.OpenWrite("text.bin"), articles); //binary file
            //taka se pravi Binary Deserialization!!!!!
            var deserializedBinaryData = (List<Article>)binarySerialiser.Deserialize(File.OpenRead("text.bin"));
            foreach (var item in deserializedBinaryData)
            {
                Console.WriteLine(item.Title);
                Console.WriteLine("    " + item.Description);
                Console.WriteLine("url: " + item.Url);
            }
        }
    }

    //class for xml serialization help, with attributes, the class is specified according C# best practices:
    [XmlType("doc")]
    [Serializable]
    public class Article
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("abstract")]
        public string Description { get; set; }

        [XmlIgnore]
        public string Url { get; set; }
    }

    //class for xml serialization help, without attributes, the class is NOT specified according C# best practices:
    public class doc
    {
        public string title { get; set; }

        public string @abstract { get; set; }

        public string url { get; set; }
    }

    class Plane
    {
        public int Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }
    }
}
