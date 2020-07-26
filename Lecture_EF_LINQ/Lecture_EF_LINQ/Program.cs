using Lecture_EF_LINQ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace Lecture_EF_LINQ
{
    class Program
    {
        static void Main()
        {
            var dbContext = new MusicXContext();

            Console.OutputEncoding = Encoding.Unicode; //za da chete kirilicata ot DB-a, koqto idwa.

            //var songs = dbContext.Songs
            //    .Where(x => x.Name.Contains("Survivor"))
            //    .OrderBy(x => x.ModifiedOn)
            //    .ThenBy(x => x.Name)
            //    .ToList();

            //var songs = dbContext.Songs
            //    .Where(x => x.Name.Contains("Survivor"))
            //    .OrderBy(x => x.ModifiedOn)
            //    .ThenBy(x => x.Name)
            //    .Select(x => new ProjectionSong
            //    {
            //        Name = x.Name,
            //        SourceName = x.Source.Name, //avtomatichen Join shte napravi EF Core, shte polzwa navigation 
            //        //propertyto Source v Song i shte otide da vzeme ot Joinnatata tablica Source Name! Towa e:
            //        //SELECT[s].[Name], [s0].[Name] AS[SourceName]
            //        //FROM[Songs] AS[s]
            //        //LEFT JOIN[Sources] AS[s0] ON[s].[SourceId] = [s0].[Id]
            //        //WHERE CHARINDEX(N'Survivor', [s].[Name]) > 0
            //        //ORDER BY[s].[ModifiedOn], [s].[Name]
            //    })
            //    .ToList(); //elementite sa veche ne ot type Song, a ot type ProjectionSong!!!

            //var songs = dbContext.Songs
            //    //.Where(x => x.Name.Contains("Survivor"))
            //    .OrderBy(x => x.ModifiedOn)
            //    .ThenBy(x => x.Name)
            //    .Select(x => new ProjectionSong
            //    {
            //        Name = x.Name,
            //        SourceName = x.Source.Name, 
            //    })
            //    .Where(x=>x.SourceName == "User")
            //    .ToList(); 

            //var songs = dbContext.Songs
            //    //.Where(x => x.Name.Contains("Survivor"))
            //    .OrderBy(x => x.ModifiedOn)
            //    .ThenBy(x => x.Name)
            //    .Select(x => new 
            //    {
            //        x.Name,
            //        SourceName = x.Source.Name,
            //    })
            //    .Where(x => x.SourceName == "User")
            //    .ToList();

            //var songs = dbContext.Songs
            //    .OrderBy(x => x.ModifiedOn)
            //    .ThenBy(x => x.Name)
            //    .Where(x => x.Source.Name == "User")
            //    .ToList();
            //foreach (var song in songs)
            //{
            //    Console.WriteLine(song.Name + "=>" + song.Source.Name); //tova gyrmi, zashtoto Source.Name ne e
            //    //vzet ot DB-a, ne e praven JOIN s tablicata Sources, poneje nqmam Select v zaqwkata i ne sym go
            //    //poiskala Source.Name!!!
            //}

            //var songs = dbContext.Songs
            //    .Include(x => x.Source) //Include e method ot EF Core i pravi JOIN na Songs s Sources!!!!
            //                            //veche ne gyrmi dolu!!! Include e baven i negotin, zashtoto vziam vsichko ot 2-ta tablici, t.e.
            //                            //generira nenujen trafic i danni. Zatowa e po-dobre da polzwam Select() - toj syshto pravi JOIN, no
            //                            //mi vryshta samo kakwoto si iskam, t.e. proection da polzwam pred Include!!!!
            //  //no Include za razlika ot Select, dobavq sybranite na edno mqsto collections, za Trackvane i te
            // //mogat da se promenqt i da im dawam SaveChanges() posle!!!!
            //    .OrderBy(x => x.ModifiedOn)
            //    .ThenBy(x => x.Name)
            //    .Where(x => x.Source.Name == "User")
            //    .ToList();

            //var songs = dbContext.Songs
            //    .OrderBy(x => x.ModifiedOn)
            //    .ThenBy(x => x.Name)
            //    .Where(x => x.SongArtists.Count() > 2)
            //    .Select(x => new
            //    {
            //        x.Name,
            //        Artists = x.SongArtists.Select(x => x.Artist.Name),
            //    })
            //    .ToList();

            //var songsCount = dbContext.Songs
            //    .OrderBy(x => x.ModifiedOn)
            //    .ThenBy(x => x.Name)
            //    .Where(x => x.SongArtists.Count() > 2)
            //    .Select(x => new
            //    {
            //        x.Name,
            //        Artists = x.SongArtists.Select(x => x.Artist.Name),
            //    })
            //    .Count(); 
            //Console.WriteLine(songsCount); //1075

            //var songsCount = dbContext.Songs
            //    .Count(x => x.SongArtists.Count() > 2);
            //Console.WriteLine(songsCount); //1075

            //var songsCount = dbContext.Songs
            //    .Where(x => x.SongArtists.Count() > 2)
            //    .Average(x=>x.Id);
            //Console.WriteLine(songsCount); //40340.82511627907

            //var songs = dbContext.Songs
            //    .Where(x => x.SongArtists.Count() > 2)
            //    .Max(x => x.Name);
            //Console.WriteLine(songs); //Толкова

            //var songs = dbContext.Songs
            //    .Where(x => x.SongArtists.Count() > 2)
            //    .Min(x => x.Name);
            //Console.WriteLine(songs); //1,2,3

            //var songs = dbContext.Songs
            //    .Where(x => x.SongArtists.Count() > 2)
            //    .Sum(x => x.Id);
            //Console.WriteLine(songs); //43366387

            //var songs = dbContext.Songs
            //    .OrderBy(x => x.ModifiedOn)
            //    .ThenBy(x => x.Name)
            //    .Where(x => x.SongArtists.Count() > 2)
            //    .Select(x => new
            //    {
            //        x.Name,
            //        FirstArtist = x.SongArtists.Select(x => x.Artist.Name).Min(),
            //        Artists = x.SongArtists.OrderBy(x => x.SongId).Select(x => x.Artist.Name),
            //    })
            //    .ToList();

            //var songs = dbContext.Songs.Join( //towa pravi INNER JOIN!!!
            //    dbContext.Sources,
            //    song => song.SourceId,
            //    source => source.Id,
            //    (song, source) => new
            //    {
            //        SongName = song.Name,
            //        SongSource = source.Name,
            //    }
            //    ).ToList();

            ////tova e syshoto kato gornoto, no mega po-lesno i kratko, no pravi LEFT JOIN:
            //var songs = dbContext.Songs
            //    .Select(x => new
            //    {
            //        SongName = x.Name,
            //        SongSource = x.Source.Name,
            //    }).ToList();

            //var songs = dbContext.Songs
            //    .GroupBy(x => x.Name.Substring(0, 1))
            //    .Select(x => new
            //    {
            //        FirstLetterKey = x.Key,
            //        CountSongsInGroup = x.Count(),
            //        FirstSong = x.Min(s=>s.Name),
            //    })
            //    .Where(x=>x.CountSongsInGroup >=1000)
            //    .ToList();

            //foreach (var song in songs)
            //{
            //    Console.WriteLine(song.FirstLetterKey + " => " + song.CountSongsInGroup + " >>> " + song.FirstSong);
            //}

            //var songs = dbContext.Songs //tova ne raboti, zashtoto EF ne dava Client site GroupBy, t.e. trqbwa da
            ////izpylnq edin Select sled GroupBy-a, za da raboti!!!!
            //    .GroupBy(x => x.Name.Substring(0, 1))
            //    .Select(x => new
            //    {
            //        x.Key,
            //        Counts = x.Count(),
            //    })
            //    .ToDictionary(x => x.Key, x => x.Counts);
            //foreach (var song in songs)
            //{
            //    Console.WriteLine(song.Key + " => " + string.Join(Environment.NewLine, song.Value));
            //}

            //var artists = dbContext.Artists
            //    .Select(a => new
            //    {
            //        ArtistName = a.Name,
            //        Songs = a.SongArtists.Select(sa => sa.Song.Name),
            //    })
            //    .ToList(); //rezultata e List<List<string>>, t.e. dvumeren List! Sys SelectMany toj moje da
            ////se naprawi na List<string>!!!
            //foreach (var a in artists)
            //{
            //    Console.WriteLine(a.ArtistName + ": " + a.Songs.Count());
            //    foreach (var s in a.Songs)
            //    {
            //        Console.WriteLine("         " + s);
            //    }
            //}

            //var artists = dbContext.Artists
            //    .SelectMany(a => a.SongArtists.Select(sa => sa.Song.Name))
            //    .ToList(); //rezultata e List<string>, t.e. ednomeren List!
            //foreach (var s in artists)
            //{
            //    Console.WriteLine(s);
            //}

            //var artists = GetData();

            //foreach (var a in artists)
            //{
            //    Console.WriteLine(a.ArtistName + ": " + a.Songs.Count());
            //    foreach (var s in a.Songs)
            //    {
            //        Console.WriteLine("         " + s);
            //    }
            //}

            //var songsGroups = dbContext.Songs
            //     .GroupBy(x => new { x.Name, x.SourceItemId }) //moga da groupiram po 2 neshta, no te ne trqbwa da 
            //                                                   //sa navigation properties!!!
            //     .Select(x => new
            //     {
            //         x.Key.Name,
            //         x.Key.SourceItemId
            //     }).ToList();
            //foreach (var a in songsGroups)
            //{
            //    Console.WriteLine(a.Name + ": " + a.SourceItemId);
            //}
            ////tozi foreach e vsyshtnost towa:
            //var enumerator = songsGroups.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    var a = enumerator.Current;
            //    Console.WriteLine(a.Name + ": " + a.SourceItemId);
            //}
            //enumerator.Dispose();

            //var songsGroups = dbContext.Songs
            //     .GroupBy(x => new { x.Name, x.SourceItemId }) //moga da groupiram po 2 neshta, no te ne trqbwa da 
            //                                                   //sa navigation properties!!!
            //     .Select(x => new
            //     {
            //         x.Key.Name,
            //         x.Key.SourceItemId
            //     }).ToList();
            //foreach (var a in songsGroups)
            //{
            //    Console.WriteLine(a.Name + ": " + a.SourceItemId);
            //}




            ////razlikata m/u Func i Expression<Func>:
            //Func<string, bool> predicate = x => x.Length > 3;
            //var a = predicate.Invoke("asdasdasd");
            //Console.WriteLine(a); //True

            //Expression<Func<string, bool>> expressionPredicate = x => x.Length > 3;
            //var a1 = expressionPredicate.Parameters;
            //Console.WriteLine(a1); //System.Collections.ObjectModel.ReadOnlyCollection`1[System.Linq.Expressions.ParameterExpression]
        }
        static IEnumerable<ArtistsWithSongsViewModel> GetData()
        {
            var dbContext = new MusicXContext();
            var artists = dbContext.Artists
                .Where(a => a.Name.StartsWith("P"))
                .Select(a => new ArtistsWithSongsViewModel
                {
                    ArtistName = a.Name,
                    Songs = a.SongArtists.Select(sa => sa.Song.Name),
                })
                .AsEnumerable();

            return artists;
        }
    }


    internal class ArtistsWithSongsViewModel
    {
        public string ArtistName { get; set; }
        public IEnumerable<string> Songs { get; set; }
    }

    class ProjectionSong
    {
        public string Name { get; set; }

        public string SourceName { get; set; }

        //public DateTime? ModifiedOn { get; set; }
    }
}
