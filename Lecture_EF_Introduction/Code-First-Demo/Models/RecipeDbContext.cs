using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code_First_Demo.Models
{
    public class RecipeDbContext : DbContext
    {
        public RecipeDbContext()
        {
        }
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options)
            : base(options)
        {
        }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) //dali e configuriran optionsBuilder-a.
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Recipes;Integrated Security=true;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>().HasIndex(x => x.Name);
            //dobavqm nov index, razlichen ot Frimare Key, kojto
            //stava avtomatichno na index, a az dobavqm nov index, kojto shte e po Name na Recipetata.
            //sega v realnata DB imam veche 3 indexa za table Ingradients:
            //[IX_Ingredients_Name] - Non-Unique, Non-Clustered
            //[IX_Ingredients_RecipeId] - Non-Unique, Non-Clustered
            //[PK_Ingredients] - Clustered

            modelBuilder.Entity<Recipe>().HasIndex(x => x.Name);
        }
    }
}
