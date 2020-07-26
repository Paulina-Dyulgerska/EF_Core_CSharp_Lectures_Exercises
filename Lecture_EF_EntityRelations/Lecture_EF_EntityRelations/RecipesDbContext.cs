using Lecture_EF_EntityRelations.EntityConfigurations;
using Lecture_EF_EntityRelations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Lecture_EF_EntityRelations
{
    public class RecipesDbContext : DbContext
    {
        public RecipesDbContext()
        {

        }

        public RecipesDbContext(DbContextOptions<RecipesDbContext> options)
            : base(options)
        {
            //tozi ctor go pravq samo ako iskam da imam vyzmojnost az da promenqm nastrojkite na provider naprimer
            //ili da imam razlichni connection strings, ot nqkolko razlichni mesta na tozi DbContext.
            //tezi options gi podawam na base ctor-a, za da se oprawq toj s tqh.
        }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Student> Students { get; set; }
        
        public DbSet<Course> Courses { get; set; }

        public DbSet<StudentCourse> StudentsCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Recipes;Integrated Security=true;");
            }
        }

        //fluent api sa vsichki oniq nastrojki, koito nie pravim v OnModelCrating()!!!
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //tova se podrazbira ot structurata na classovete i attributite im, ne nujno da go pisha i tuk, no
            //mojeshe samo tuk da go pisha, ako v classes i attributes ne mi e opraveno, no zaradi Delete, az ne
            //moga da opisha vsichko s attributes i trqbwa da go napisha tuk taka, inache nqma da moga da
            //definiram OnDelete kakwo pravi, zaradi nego trqbwa da opisha i samite vryzki, za koito se
            //otnasq tozi OnDelete!!!!:
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Courses)
                .WithOne(c => c.Student)
                .HasForeignKey(s=>s.StudentId)
                .OnDelete(DeleteBehavior.Cascade); //pri iztrivane na Student, vsichki zapishi v StudentsCourses
            //shte bydat iztriti.

            //tova se podrazbira ot structurata na classovete i attributite im, ne nujno da go pisha i tuk, no
            //mojeshe samo tuk da go pisha, ako v classes i attributes ne mi e opraveno, no zaradi Delete, az ne
            //moga da opisha vsichko s attributes i trqbwa da go napisha tuk taka, inache nqma da moga da
            //definiram OnDelete kakwo pravi, zaradi nego trqbwa da opisha i samite vryzki, za koito se
            //otnasq tozi OnDelete!!!!:
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithOne(s => s.Course)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade); //pri iztrivane na Course, vsichki zapishi v StudentsCourses
            //shte bydat iztriti.

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId }); //Composite Primary Key

            modelBuilder.Entity<StudentCourse>()
              .HasOne(sc => sc.Student)
              .WithMany(s => s.Courses)
              .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
              .HasOne(sc => sc.Course)
              .WithMany(s => s.Students)
              .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(x => x.Name).HasMaxLength(100); // [MaxLength(100)]
            });

            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Address)
                .WithOne(x => x.Employee)
                .OnDelete(DeleteBehavior.Restrict);

            //izneseni sa nastrojkite za tablica Recipes v RecipeConfigurations.cs file!!!!
            modelBuilder.ApplyConfiguration(new RecipeConfigurations());

            //izneseni sa nastrojkite za tablica Recipes v RecipeConfigurations.cs file!!!!
            //modelBuilder.Entity<Recipe>(entity => //kazwam za koq tablica se otnasqt promenite, shte e table Recipes zasegnata!!
            //{
            //    entity.Property(x => x.Name).HasColumnName("Title");
            //    //sega vmesto Name, kolonata zad property Name shte se kazwa Title v DB-a mi!!!

            //    //.HasColumnType("char(30)") //moga da smenq defaultniq type na colonata Name ot tuk.
            //    //no obiknoveno e typo taka da se vyrja s konkreten type na konkreten type DB, zashtoto ne sym 
            //    //flexy ako smenq bazata utre. zatowa pisha taka:
            //    //.HasColumnType("varchar") //stremq se kym abstraction i v izbora na type - sega iskam NVARCHAR(100).
            //    //no mi hvyrlq greshka i ne dawa da se pravi sega promqna v type na Title colonata. Da vnimawam kak go
            //    //omotwam v tezi types, che sega mi iska da naprawi varchar(1) samo da e colonata.

            //    entity.Property("Name") //kazwam za koe property prawq nadolu promenite!!!!
            //    .HasMaxLength(100)
            //    .IsUnicode(true)
            //    .IsRequired(true); //ne moje da e NULL vypreki, che type go pozwolqwa, zashtoto e nullable stringa.

            //    entity.HasKey("Name");
            //    //moga da si pisha koqto si iskam kolona da e PK!!!

            //    entity.ToTable("MyRecipes", "system");
            //    //shte smenq imeto na tablicata ot Recipes na MyRecipes!!! Sega v DB imam:
            //    //[system].[MyRecipes] vmesto [dbo].Recipes!!!!
            //    //t.e. smebih i schemata, v koqto e tablicata.

            //    //entity.HasNoKey();  //ako iskam da nqma Primary Key edna table, az trqbwa izrishno da kaja towa
            //    //taka sega tablicata ostawa bez Primary Key, no ot tuk natatyk tablicata NE moje da se trackva
            //    //ot EF Core i zabrawqm za promeni v neq!!!! Moga samo da q cheta!!!
            //    //zatowa po princip ne iskam da prawq takiwa nestha, tochno zashtoto EF nqma kak da znae koe da trie i 
            //    //obrabotwa, ako nqma id ili index, po kojto da sledi koe zapische trqbva da se promeni.

            //    entity.Ignore("Test");
            //    //propertyto Test ne go iskam da e kolona v DB-a!!!! TO shte si ostane samo 
            //    //kato localno property i nqma da wliza w modela.

            //    //    //Disabling cascade delete:
            //    //    //If a FK property is non - nullable, cascade delete is on by default:
            //    //    //modelBuilder.Entity<Course>()
            //    //    //  .HasRequired(t => t.Department)
            //    //    //  .WithMany(t => t.Courses)
            //    //    //  .HasForeignKey(d => d.DepartmentID) //Throws exception on delete
            //    //    //  .OnDelete(DeleteBehavior.Restrict);
            //    //    entity.HasMany(x => x.Ingredients)
            //    //    .WithOne(x => x.Recipe)
            //    //    //.OnDelete(DeleteBehavior.Cascade) //razreshavam cascadno iztrivane, t.e. triq receptata i se triqt
            //    //    //vsichki vyrzani za neq ingredients!!! towa ne go iskam i e opasno - moga da si iztriq cqlata DB!!!
            //    //    .OnDelete(DeleteBehavior.Restrict); //zabranqwam iztriwane na recepta, dokato e
            //    //    //s vyrzani za neq Ingredients - trqbwa da sloja null v ingredientite i posle da triq recipe-to!!!
            //    //    //Throws exception on delete
            //    //     //.OnDelete(DeleteBehavior.SetNull); //ako e nullable colonata, koqto e FK, moga da razresha da i
            //    //    //se setne null, t.e. v property Recipe na Ingredientite koito imat tazi Recipe zakachena, shte se 
            //    //    //zapishe null pri delete-vaneto na tazi Recipe i nqma da mrynka DB-a za nishto.
            //});
        }
    }
}
