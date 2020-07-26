using Microsoft.EntityFrameworkCore;

namespace Lecture_ORM_Fundamentals.Models
{
    public class StudentsDBContext : DbContext
    {
        //tozi file mi pravi shemata na database, sybira wsichki tablici, opiswa gi i reshawa kak da se kazwat kolonite i t.n.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=GradesDB;Integrated Security=true;");
            //towa mi e connection stringa.
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(p => p.Property(x => x.LastName).IsRequired()); //towa znachi che LastName propertyto
            //ne moje da e null na studenta!!! Po podrazbirane ORMa shte mi go naprawi da moje da e NULL!!!
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Course> Courses { get; set; }

    }
}
