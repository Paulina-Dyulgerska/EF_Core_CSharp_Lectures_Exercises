//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using Lecture_EF_Auto_Mapping_Object.Models;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Lecture_EF_Auto_Mapping_Object
//{
//    class Program
//    {
//        static void Main()
//        {
//            Console.OutputEncoding = Encoding.Unicode;

//            var db = new MusicXContext();

//            ////primer za MCV project organisation!!!!!!!
//            ////Service-to po princip mi se dava otvyn ot nqkoj, no sega nqma kak i zatowa az si go syzdawam.
//            ////vzimam si Service-to:
//            //var service = new ArtistsServise(new MusicXContext()); //Service raboti s DB-a chrez Modelite, koito EF Core mi e
//            ////generiral - te mu idwat v MusicXContext!!!
//            //var artists = service.GetAllWithCount(); //vzimam si ot Service-to moite DTO-ta. Serviceto byrka s Models, a
//            ////Models byrkat v DB-a!!!!
//            ////PrintArtists(artists); //podawam na View-to DTO-tata ot Service Layer-a!!! View-to vizualizira informaciqta.
//            //PrintArtistsAsJson(artists); //taki pravq View po syvsem nov nachin.

//            //ako i manual da si pravq mapping, nikoga da ne zabrawqm, che navigational propertytata NE se popylvat, ako ne gi
//            //vzema sys Select ili ako nqmam nqkakyw vid Loading specificiran!!!
//            //List<ArtistWithCountViewModel> artists = db.Artists.Select(x => new ArtistWithCountViewModel
//            //{
//            //    Name = x.Name,
//            //    SongsCount = x.SongArtists.Count(),
//            //}).ToList();

//            //4 steps for mapping:
//            //var config = new MapperConfiguration(cnf => //configuriram Mapper-a
//            //{
//            //    cnf.CreateMap<Artists, ArtistWithCountViewModel>(); //kazwam mu: Kakvoto pravish sam, towa e!
//            //    //syzdawa vryzka m/u model classa Artists i view classa ArtistWithCountViewModel
//            //    //t.e. kazwam mu mapper, nauchi se da konvertirash ot Artists v ArtistWithCountViewModel!!!
//            //    //kakvo shte naprawi mapper-a? shte priloji vsichki ekvilibristiki, koito znae, za da obyrne ot ediniq class v
//            //    //drugiq class!! Edno ot nomerata mu e: ako ime i type na property ot Artists syvpada s ime i type na property ot
//            //    //ArtistWithCountViewModel, to znachi direktno gi prenasq ot Source class-a kym Target class-a.
//            //    //no mapper-a ne znae kakwo da naprawi s propertyto SongsCount ot ArtistWithCountViewModel i tam shte go ostavi da
//            //    //e 0!!!!
//            //    //tuk moga da slagam vryzki za vsichki models kym viewmodel vryzki, koito mi trqbwat v ramkite na prilojenieto mi:
//            //    cnf.CreateMap<Songs, SongViewModel>() //kazwam mu: Pravi kakvoto pishe tuk, zashtoto ne mojesh sam!
//            //        .ForMember(
//            //            x => x.Artists,
//            //            opt => opt.MapFrom(
//            //                s => string.Join(", ", s.SongArtists.Select(a => a.Artist.Name))))
//            //        .ForMember(
//            //            x => x.LastModified,
//            //            opt => opt.MapFrom(
//            //                s => s.ModifiedOn ?? s.CreatedOn)) //s towa kazah: za propertyto LastModified izpolzwam ModifiedOn, no ako 
//            //                                                   //ModifiedOn e null, to izpolzwaj CreatedOn, zashtoto ako nqma posledna modification, to e logichno creat datata da se
//            //                                                   //qwqwa posledna modification.
//            //        .ReverseMap(); //kazwam mu: Towa, koqto sym ti zadala sega, shte trqbwa da mojesh da go pravish i na obratno!
//            //    //s towa shte moga da pravq obraten mapping - ot inputModel da pravq object ot type Songs!!!!
//            //});

//            //nov variant na gornoto, no s izpolzwane na Profile:
//            var config = new MapperConfiguration(cnf => 
//            {
//                cnf.CreateMap<Artists, ArtistWithCountViewModel>();
//                cnf.AddProfile(new SongsToViewModelProfile());
//            });

//            IMapper mapper = config.CreateMapper(); //tozi config mi dava instanciq na AutoMapper!

//            //Artists artist = db.Artists.FirstOrDefault(x => x.Id == 10);

//            //moga da polzwam mapper-a, za da mi napravi ot edin obekt drug - ot artist da mi napravi ArtistWithCountViewModel:
//            //ArtistWithCountViewModel artistViewModel = mapper.Map<ArtistWithCountViewModel>(artist);
//            //towa e obiknoven manual mapping.

//            //moga direktno taka da si gi vzimam nesthata ot mapper-a (toj ne znam dali ne e neobiknoven:)). Tuk ProjectTo()
//            //pravi za men Select() i vzima towa, koeto mi trqbwa za DTO class-a ot DB-a!!!!
//            ArtistWithCountViewModel artistViewModel = db.Artists.Where(x => x.Id == 10)
//                .ProjectTo<ArtistWithCountViewModel>(config).FirstOrDefault();
//            //tazi zaqwka se pravi otdolu ot AutoMapper-a, kogato polzwam ProjectTo():
//            //SELECT TOP(1) [a].[CreatedOn], [a].[ModifiedOn], [a].[Name], (
//            //                SELECT COUNT(*)
//            //                FROM[SongArtists] AS[s]
//            //                WHERE[a].[Id] = [s].[ArtistId]) AS[SongArtistsCount]
//            //            FROM[Artists] AS[a]
//            //            WHERE[a].[Id] = 10

//            //Print(artist);
//            //Print(artistViewModel);

//            //List<SongViewModel> songsInViewModel = db.Songs.ProjectTo<SongViewModel>(config).ToList();
//            //Print(songsInViewModel);

//            ////Dobra praktika e da izbqgwam tozi podhod na manual mapping, zashtoto
//            ////ako imam navigation property v DTO classa, dori i da sym kazala v config - a
//            ////ot kyde shte se  wzima to, kogato ne e s ProjectTo() vzet obekta ot DB-a, 
//            ////az nqma da polucha neshtata zapisani v navigational propertytata!!!!
//            ////primer zashto da ne go prawq:
//            //IMapper mapperManual = config.CreateMapper();
//            //Songs song = db.Songs.Skip(9).FirstOrDefault();
//            //SongViewModel songViewModel = mapper.Map<SongViewModel>(song);
//            //Print(songViewModel);
//            //          {
//            //              "Name": "What It Feels Like For A Girl",
//            //"SongArtistsCount": 0,
//            //"Artists": "",
//            //"SourceName": null,
//            //"LastModified": "2018-10-01T13:41:07.7718932"
//            //          }

//            //dokato tova vzima wsichko ot navigational propertytata:
//            SongViewModel songViewModel = db.Songs.Skip(9).ProjectTo<SongViewModel>(config).FirstOrDefault();
//            Print(songViewModel);
//            //          {
//            //              "Name": "What It Feels Like For A Girl",
//            //"SongArtistsCount": 1,
//            //"Artists": "Madonna",
//            //"SourceName": "Top40Charts",
//            //"LastModified": "2018-10-01T13:41:07.7718932"
//            //          }

//            ////Unflattening Complex Objects:
//            //var inputModel = new SongViewModel { Name = "Test123", SourceName = "VBox7" };
//            ////ako dobavq ReverseMap() pri CreateMap()-a, az shte aktiviram obratniq mapping i shte moga 
//            ////gorniq inputModel da go naprawq na obekt ot type Songs!!!!
//            //Songs song = mapper.Map<Songs>(inputModel);
//            //Print(song);
//            ////syzdade mi nov obekt ot type Songs i popylni v nego kakwoto moja kato danni, drugoto ostavi null ili defaultnoto za tipa danni,
//            ////ako v dadenata kletka e slojeno NOT NULL v DB-a!!!
//            ////{
//            ////  "Id": 0,
//            ////  "CreatedOn": "0001-01-01T00:00:00",
//            ////  "ModifiedOn": null,
//            ////  "IsDeleted": false,
//            ////  "DeletedOn": null,
//            ////  "Name": "Test123",
//            ////  "SourceId": null,
//            ////  "SourceItemId": null,
//            ////  "SearchTerms": null,
//            ////  "Source": {
//            ////                    "Id": 0,
//            ////    "CreatedOn": "0001-01-01T00:00:00",
//            ////    "ModifiedOn": null,
//            ////    "Name": "VBox7",
//            ////    "ArtistMetadata": [],
//            ////    "SongMetadata": [],
//            ////    "Songs": []
//            ////  },
//            ////  "SongArtists": [],
//            ////  "SongMetadata": []
//            ////}
//            //db.Songs.Add(song);
//            //db.SaveChanges();

//            //ako iskam da imam ProjectTo() no ne iskam da pravq mapping configuration i da go poddyrjam:
//            //polzwam na Niki template:

//        }

//        public static void Print(object artists)//towa mi e druga View chast, drug UI!!!!
//        {
//            Console.WriteLine(JsonConvert.SerializeObject(artists, Formatting.Indented));
//        }

//        public static void PrintArtistsAsJson(IEnumerable<ArtistWithCountViewModel> artists)//towa mi e druga View chast, drug UI!!!!
//        {
//            Console.WriteLine(JsonConvert.SerializeObject(artists, Formatting.Indented));
//        }

//        public static void PrintArtists(IEnumerable<ArtistWithCountViewModel> artists) //towa mi e View chastta
//        {
//            foreach (var artist in artists)
//            {
//                Console.WriteLine(($"~~~~{artist.Name}~~~~ => {artist.SongArtistsCount} " +
//                    $"song{(artist.SongArtistsCount > 1 ? "s" : string.Empty)}"));
//                //towa e presentational logic.
//            }
//        }
//    }

//    class SongViewModel
//    {
//        public string Name { get; set; }

//        public int SongArtistsCount { get; set; }

//        public string Artists { get; set; }  //automapper shte go sloji = 0, zashtoto ne moje da go navyrje s nishto ot classa
//        //Songs. Zatowa az trqbwa da specificiram kakwo da se wzeme tuk.

//        public string SourceName { get; set; } //taka mapper-a otiva i ot Songs otiva v navigational propertyto Source i mu vzima propertyto Name!!!!

//        public DateTime LastModified { get; set; }

//    }

//    class ArtistWithCountViewModel //towa e DTO, to mi pokazwa samo vajnoto za View-to, ne vsichkata informaciq, koqto imam!!!!
//    {
//        public string Name { get; set; }

//        //public int SongsCount { get; set; } //s tova ime mapper-a ne znae kakwo da mi dade i mi vryshta 0, no ako go napravq taka:
//        public int SongArtistsCount { get; set; } //sega avtomatichno mapper-a shte mi vryshta broq na songs v collectiona SongArtists na vseki artist!!!

//        public DateTime CreatedOn { get; set; }

//        public DateTime? ModifiedOn { get; set; }

//    }
//}
