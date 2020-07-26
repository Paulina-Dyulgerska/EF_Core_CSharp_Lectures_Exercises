using Code_First_Demo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Code_First_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new RecipeDbContext();
            //db.Database.EnsureCreated();
            db.Database.Migrate(); //skrivam EnsureCreated() i puskam towa, kogato iskam da Runna Migration!!!!
            //db.Recipes.Add(new Recipe { Name = "Musaka" });
            ////addnah si tova:
            //RecipeId Name
            //1   Musaka

            ////tova e cascadno zapisvane na danni, spisyka s Ingredients se dobava v DB-a i v tablica Recipe, i v
            ////tablica Ingredients.
            //for (int i = 0; i < 100; i++)
            //{
            //    db.Recipes.Add(new Recipe
            //    {
            //        Name = i.ToString(),
            //        Ingredients = new List<Ingredient>
            //        {
            //            new Ingredient{ Name = "Meat", Amount = 540},
            //            new Ingredient{ Name = "Pottatos", Amount = 450},
            //        },
            //    });
            //}

            //moga da pravq Remove:
            //db.Recipes.Remove(db.Recipes.Where(x => x.RecipeId == 1).FirstOrDefault());

            //moga da iztriq i taka bez da pravq vzimane na  realna danna ot DB-a:
            //db.Recipes.Remove(new Recipe { RecipeId = 102 }); //no ako nqma takowa Id, shte mi throwne exception!!!

            //db.SaveChanges();

        }
    }
}
