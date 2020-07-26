using Lecture_EF_EntityRelations.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Lecture_EF_EntityRelations
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new RecipesDbContext();
            //moga da si pusna chista SQL zaqwka prez db!!! Ne mi trqbwa ADO.NET da polzwam, a direktno towa:
            //db.Database.ExecuteSqlRaw(""); //s towa si puskam chisti zaqwki napisani na SQL!!!

            db.Database.EnsureDeleted(); //ako ima takawa DB, izptrij mi q.
            db.Database.EnsureCreated(); //syzdaj mi tazi DB na nowo.

            db.Recipes.Add(new Recipe
            {
                Name = "Musaka",
                Description = "Traditional Bulgarian meat dish.",
                CookingTime = new TimeSpan(2, 30, 45),
            });

            ////chetene na shadow property:
            //var a = db.Recipes.Select(x => new
            //{
            //    Eng = EF.Property<string>(x, "EGN"),
            //}).FirstOrDefault();
            ////Console.WriteLine(a.Eng);

            //tova pravi zapis na 4 rows v 3 razlichni tablici samo za wkarwaneto na 1 student!!!!
            var student = new Student
            {
                FirstName = "Polq",
                LastName = "Poleva",
                Courses =
                {
                    new StudentCourse
                    {
                        Course = new Course{ Name = "C#"}
                    },
                    new StudentCourse
                    {
                        Course = new Course{Name = "C#DBs"}
                    }
                }
            };

            db.Students.Add(student);

            db.SaveChanges();
        }
    }
}
