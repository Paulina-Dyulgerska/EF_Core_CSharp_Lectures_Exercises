using AutoMapper;
using AutoMapper.QueryableExtensions;
using Lecture_EF_Auto_Mapping_Object.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lecture_EF_Auto_Mapping_Object
{
    class Program
    {
        static void Main()
        {
            //ako iskam da imam ProjectTo() no ne iskam da pravq mapping configuration i da go poddyrjam.
            //polzwam na Niki template v GitHub - ASP.NET-Core-Template/src/Services/AspNetCoreTemplate.Services.Mapping/:

            Console.OutputEncoding = Encoding.Unicode;

            AutoMapperConfig.RegisterMappings(Assembly.GetExecutingAssembly());
            //tuk pochva da se minawa po vsichki classove i se pravi mappinga na vseki class sys syotvetnite nastrojki.
            //minava i tyrsi koi classes imam IMapTo, IMapFrom i IHaveCustomMappings interfaces i im sybira mappingite i gi
            //configurira!!!

            var db = new MusicXContext();

            var songs = db.Songs.To<SongViewModel>().Take(10).ToList();

            Print(songs);


        }

        public static void Print(object artists)//towa mi e druga View chast, drug UI!!!!
        {
            Console.WriteLine(JsonConvert.SerializeObject(artists, Formatting.Indented));
        }
    }

    class SongViewModel : IMapFrom<Songs>, IHaveCustomMappings, IMapTo<SongViewModel>
    {
        public string Name { get; set; }

        public int SongArtistsCount { get; set; }

        public string Artists { get; set; }  //automapper shte go sloji = 0, zashtoto ne moje da go navyrje s nishto ot classa
        //Songs. Zatowa az trqbwa da specificiram kakwo da se wzeme tuk.

        public string SourceName { get; set; } //taka mapper-a otiva i ot Songs otiva v navigational propertyto Source i mu vzima propertyto Name!!!!

        public DateTime LastModified { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Songs, SongViewModel>()
                    .ForMember(
                        x => x.Artists,
                        opt => opt.MapFrom(
                            s => string.Join(", ", s.SongArtists.Select(a => a.Artist.Name))))
                    .ForMember(
                        x => x.LastModified,
                        opt => opt.MapFrom(
                            s => s.ModifiedOn ?? s.CreatedOn))
                    .ReverseMap();
        }
    }

    class ArtistWithCountViewModel : IMapFrom<Artists>, IHaveCustomMappings, IMapTo<ArtistWithCountViewModel>
    {
        public string Name { get; set; }

        public int SongArtistsCount { get; set; } //sega avtomatichno mapper-a shte mi vryshta broq na songs v collectiona SongArtists na vseki artist!!!

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Artists, ArtistWithCountViewModel>();
        }
    }
}
