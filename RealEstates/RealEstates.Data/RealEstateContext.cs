using Microsoft.EntityFrameworkCore;
using RealEstates.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Data
{
    public class RealEstateContext : DbContext
    {
        public RealEstateContext()
        {
        }

        public RealEstateContext(DbContextOptions options)
            : base(options)
        {
            //ideqta na tozi constructor e, che nqkoj moje da iska otvyn da mi podade razlichni options ot moite,
            //a ako gi podade, shte trqbwa da izpolzwam tqh, a nqma da polzwam defaultnite, zadadeni ot men v
            //methoda OnConfiguring(DbContextOptionsBuilder optionsBuilder).
            //za tezi options stawa wypros:
            //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            //{
            //    if (!optionsBuilder.IsConfigured)
            //    {
            //        optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=RealEstate;Integrated Security=true;");
            //    }
            //}
        }

        //samo kato opisha tozi DbSet, vsichki ostanali shte se zakachat v DB-a i shte se napravqt na tablici,
        //dori i da ne gi opisha az izlishno tuk, no az gi opiswam, za da ima dostyp klienta na moeto prilojenie
        //do dannite ot razlichnite tablici direktno, a ne da trqbwa da minawa vinagi prez tablicata
        //RealEstateProperties i prez neq da stiga do tablica Tags naprimer!!!! Inache beshe dostatychno samo
        //public DbSet<RealEstateProperty> RealEstateProperties { get; set; } da si napisha i vsichko shteshe da
        //mi se syzdade v DB-a, no e typo taka!
        public DbSet<RealEstateProperty> RealEstateProperties { get; set; }

        public DbSet<BuildingType> BuildingTypes { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<PropertyType> PropertyTypes { get; set; }

        //Tazi mejdinnata moje da ne q dobawqm izlishno, zashtoto nqma da q polzwam direktno. Tq shteshe da
        //si stane avtomatichno taka ili inache v DB-a - i bez da sym q deklarirala tuk:
        public DbSet<RealEstatePropertyTag> RealEstatePropertyTags { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=RealEstate;Integrated Security=true;");
            //    //TODO ?????
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RealEstatePropertyTag>(e =>
            {
                e.HasKey(x => new { x.RealEstatePropertyId, x.TagId });
            });

            //zabranqwam kogato se iztrie RealEstateProperty, da se iztrie i kvartala, v kojto se namira imota:
            modelBuilder.Entity<RealEstateProperty>()
                .HasOne(x => x.District)
                .WithMany(x => x.RealEstateProperties)
                .OnDelete(DeleteBehavior.Restrict);

            //zabranqwam kogato se iztrie District, da se iztriqt vsichki imoti, koito sa bili slojeni v nego,
            //dokato tezi imoti ne bydat prehvyrleni v drug kvartal, t.e. shte se iztrie kvartala samo, ako 
            //nqma imoti, koito da sochat kym nego v DB-a:
            modelBuilder.Entity<District>()
                .HasMany(x => x.RealEstateProperties)
                .WithOne(x => x.District)
                .OnDelete(DeleteBehavior.Restrict);
            //ako bqh dala DeleteBehavior.Cascade, tova znachi, che kogato iztriq kvartal, towa shte iztrie i
            //vsichki imoti, koito se namirat v nego spored DB-a!!! Ama towa e typo malko, zatowa ne go pravq taka.
        }
    }
}
