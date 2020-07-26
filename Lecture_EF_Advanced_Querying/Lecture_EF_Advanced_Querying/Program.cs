using Lecture_EF_Advanced_Querying.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using Z.EntityFramework.Plus;

namespace Lecture_EF_Advanced_Querying
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            var dbContext = new MusicXContext();

            ////to prevent SQL injection, use parameters!
            ////var searchedString = "_bv%";
            //var searchedString = " ' or 1=1 --"; //tozzi string bi napravil SQL Injection!!!
            ////var searchedString = Console.ReadLine();
            //var songs = dbContext.Songs
            //    //.FromSqlRaw("SELECT * FROM [Songs] WHERE [Name] LIKE ' ' or 1=1 --") //SQL Injection!!!
            //    //.FromSqlRaw("SELECT * FROM [Songs] WHERE [Name] LIKE '_bv%'") //v '' se pishe string vytre, no ako 
            //    //e s formating {0}, ne se pishe '{0}', a se ostawq {0}!!!! Taka:
            //    //.FromSqlRaw("SELECT * FROM [Songs] WHERE [Name] LIKE {0}", searchedString)
            //    //moje i s Interpolation niz, no toj pozwolqwa SLQ Injection!!!!:
            //    //.FromSqlRaw($"SELECT * FROM [Songs] WHERE [Name] LIKE '{searchedString}'")
            //    //towa pazi ot SQL Injection!!!!! Trqwba da izbiram FromSqlInterpolated, a ne FromSqlRaw:
            //    .FromSqlInterpolated($"SELECT * FROM [Songs] WHERE [Name] LIKE '{searchedString}'")
            //    .ToList();
            //foreach (var song in songs)
            //{
            //    Console.WriteLine(song.Name);
            //}

            ////taka shte predotvrati SQL Injection.
            //dbContext.Database
            //    .ExecuteSqlInterpolated($"SELECT * FROM [Songs] WHERE [Name] LIKE '{searchedString}'");

            ////taka shte stane SQL Injection.
            //dbContext.Database
            //    .ExecuteSqlRaw($"SELECT * FROM [Songs] WHERE [Name] LIKE '{searchedString}'");

            //Executing a Stored Procedure :
            //Stored Procedures can be executed via SQL:
            //            CREATE PROCEDURE UpdateAge @param int
            //            AS
            //UPDATE Employees SET Age = Age + @param;

            //var ageParameter = 5;
            //var query = "EXEC UpdateAge @age";
            //dbContext.Database.ExecuteSqlCommand(query, ageParameter); //taka vikam Stored Procedure da se izpylni.
            //dbContext.Database.ExecuteSqlInterpolated($"EXEC UpdateAge {ageParameter}"); //taka vikam Stored Procedure da se izpylni.


            //var songs = dbContext.Songs
            //    .FromSqlRaw($"SELECT * FROM [Songs] WHERE [Name] LIKE '_bv%'") //tova minava
            //    //.FromSqlRaw($"SELECT Name FROM [Songs] WHERE [Name] LIKE '_bv%'") //tova gyrmi, zashtoto
            //    //pri selecta NE vzimam vsichki requered parameters, koito ima classa Song!!!
            //    .ToList();
            //foreach (var song in songs)
            //{
            //    Console.WriteLine(song.Name);
            //}

            //var dbContext1 = new MusicXContext();
            //var song = dbContext
            //    .Songs
            //    //.AsNoTracking() //nqma da se trackva ot rezultata!!!!
            //    .FirstOrDefault(x => x.Name.StartsWith("Осъдени души"));
            ////towa prawi obekt ot type Song, i obekta song se sledi veche ot CHangeTracker i
            ////promenite po nego, koito az pravq, mogat 
            ////da bydat syhraneni v DB s SaveChanges()!!!
            //dbContext.Entry(song).State = EntityState.Detached; //nqma da se trackva poveche song!!!!
            //song.Name = "Осъдени души - ремикс"; //ako e detached obekta, tazi promqna nqma da se kachiv DB-a!!
            //dbContext.SaveChanges(); //update zaqwka se generira i se zapisvat promenite v DB-a!!!
            ////moga da zakacha obekt ot dbContext da go sledi dbContext1, dbContext pak si go sledi paralelno:
            //dbContext1.Entry(song).State = EntityState.Modified; //zakacham song na dbContext1
            ////poneje dbContext1 ne znae nishto za song, to ne znae i kakwo mu e promeneno, zatowa kato mu dam 
            ////SaveChanges, to dbContext1 shte zapishe absolyutno vsichko ot song nanovo v DB-a!!!
            ////dokato dbContext znae za song i bi zapisal samo konkretnite promeni v DB-a!!!
            //dbContext1.SaveChanges();

            //dbContext.SongMetadata.Where(x => x.Id <= 10).Delete(); //tova shte iztrie vsichki danni za 
            ////youtube adresa na songs s id <=10. tozi Delete() raboti v/u IQueryable!!!

            //dbContext.Songs.Where(x => x.Name.Contains("love"))
            //    .Update(song => new Songs { Name = song.Name + "(a love song)" });

            //var song = dbContext.Songs.FirstOrDefault(x => x.Name.StartsWith("Осъдени души"));
            //navigational propertytata na song sa Source, SongArtists, SongMetadata - po default
            //kogato vzema song, tezi propertyta ne se zarejdat, te sa null ili collectionite sa prazni!!!
            ////zatowa mi gyrmi ili ne mi vrystha nishto tozi cod, kogato vikna neshto, koeto e null ili empty!!!
            //Console.WriteLine(song.Name);
            //Console.WriteLine(song.Source.Name);
            //Console.WriteLine(song.Source);
            //Console.WriteLine(song.SongArtists);
            //Pyrvi variant da vzema towa, koeto iskam e da naprawq proektion:
            //var song = dbContext.Songs
            //    .Where(x => x.Name.StartsWith("Осъдени души"))
            //    .Select(x=> new { x.Name, x.SongArtists, x.SongMetadata, Source = x.Source.Name })
            //    .FirstOrDefault();
            //Console.WriteLine(song.Name);
            //Console.WriteLine(song.Source);
            //Console.WriteLine(song.SongArtists);
            //Console.WriteLine(song.SongMetadata);

            //var song = dbContext.Songs.FirstOrDefault(x => x.Name.StartsWith("Осъдени души"));
            //dbContext.Entry(song).Reference(x => x.Source).Load(); //tova zarejda Source navigation propertyto.
            //dbContext.Entry(song).Collection(x => x.SongMetadata).Load(); //tova zarejda Source navigation propertyto.
            //Console.WriteLine(song.Source.Name);
            //Console.WriteLine(string.Join(",", song.SongMetadata));

            //var song = dbContext.Songs
            //    .Include(x=>x.Source)
            //    .ThenInclude(x=>x.Songs) //izbiram koe navigation property na Source da includna syshto!!!!!
            //    .Include(x=>x.SongArtists)
            //    .ThenInclude(x=>x.Artist) //izbiram koe navigation property na SongArtists da includna syshto!!!!!
            //    .FirstOrDefault(x => x.Name.StartsWith("Осъдени души"));
            //Console.WriteLine(song.Source.Name);
            //Console.WriteLine(song.SongArtists);

            //var song = dbContext.Songs
            //    .FirstOrDefault(x => x.Name.StartsWith("Осъдени души"));
            //Console.WriteLine(song.Name);
            //Console.WriteLine(song.Source.Name);
            //Console.WriteLine(song.SongArtists);
            //Console.WriteLine(song.SongMetadata);

            ////tova pravi 150 000 zaqwki s Lazy Loading:
            //var songs = dbContext.Songs
            //    .Where(x => x.Name.Contains("а") || x.Name.Contains("е")) //tyrsq songs imat BG bukvi "a" ili "e"
            //    .ToList();
            //foreach (var song in songs)
            //{
            //    Console.WriteLine($"{ song.Name} => {song.Source.Name} => {song.SongArtists.Count}");
            //}

            ////tova pravi syshtoto kato gornoto, no samo s 1 zaqwka i bez Lazy Loading:
            //var songs = dbContext.Songs
            //    .Select(x=> new 
            //    { 
            //        x.Name,
            //        SourceName = x.Source.Name,
            //        SongArtistsCount = x.SongArtists.Count,
            //    })
            //    .ToList();
            //foreach (var song in songs)
            //{
            //    Console.WriteLine($"{ song.Name} => {song.SourceName} => {song.SongArtistsCount}");
            //}


            ////Concurrency Default behaiviour:
            //var song = dbContext.Songs
            //    .Where(x => x.Name.Contains("осъдени"))
            //    .FirstOrDefault();
            //song.Name = song.Name + "1"; //ochakvam Осъдени души1

            //var dbContextConcurrency = new MusicXContext();
            //var songConcurrency = dbContextConcurrency.Songs
            //    .Where(x => x.Name.Contains("осъдени"))
            //    .FirstOrDefault();
            //songConcurrency.Name = songConcurrency + "2";  //ochakvam Осъдени души2

            //dbContext.SaveChanges();
            //dbContextConcurrency.SaveChanges();
            ////nakraq shte imam Осъдени души2 w DB-a. Last one Wins - towa pravi EF i SQL-a.
            ////zashtoto song ne e spodelena m/u dvata dbContexta!!!! song e edno neshto v ediniq, drugo neshto v
            ////drugiq dbContext!!!! a dvata dbContexta sa q vzeli song samo s Осъдени души za Name, t.e. te
            ////paralelno shte q promenqt, kato poslednata promqna shte ostane za postoqnna w DB-a!!!!!

            ////ConcurrencyCheck:
            //var song = dbContext.Songs
            //    .Where(x => x.Name.Contains("love"))
            //    .FirstOrDefault();
            //song.Name = song.Name + "1"; //ochakvam Осъдени души1

            //var dbContextConcurrency = new MusicXContext();
            //var songConcurrency = dbContextConcurrency.Songs
            //    .Where(x => x.Name.Contains("love"))
            //    .FirstOrDefault();
            //songConcurrency.Name = songConcurrency.Name + "2";  //ochakvam Осъдени души2

            //dbContext.SaveChanges();

            ////dbContextConcurrency.SaveChanges(); //shte hvyrli error, che iskam i az da pisha v Name, pri
            ////uslovie, che nqkoj e promenil Name dokato az rabotq s nego. t.e. First-Wins stava tuk. Zatowa go slagam
            ////v try-catch block.

            //try
            //{
            //    dbContextConcurrency.SaveChanges(); //shte hvyrli error, che iskam i az da pisha v Name, pri
            //                                        //uslovie, che nqkoj e promenil Name dokato az rabotq s nego. t.e. First-Wins stava tuk.
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    var dbContext3 = new MusicXContext();
            //    songConcurrency = dbContext3.Songs
            //        .Where(x => x.Name.Contains("love"))
            //        .FirstOrDefault();
            //    songConcurrency.Name = songConcurrency.Name + "2";
            //    dbContext3.SaveChanges();
            //}



        }
    }
}
