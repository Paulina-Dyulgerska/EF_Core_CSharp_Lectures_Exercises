using System;
using Microsoft.Data.SqlClient;
using Dapper;

namespace DapperDemo
{
    class Program
    {
        static void Main()
        {
            using (var connection = new SqlConnection(@"Server=.\SQLEXPRESS;Database=MusicX;Integrated Security=true;"))
            {
                var songs = connection.Query<SongInfo>("SELECT TOP 10 Name, Id FROM Songs WHERE Id > 500.");

                foreach (var song in songs)
                {
                    Console.WriteLine(song.Name + " => " + song.Id);
                }
            }
        }
    }

    public class SongInfo
    {
        public string Name { get; set; }

        public int Id { get; set; }
    }
}
