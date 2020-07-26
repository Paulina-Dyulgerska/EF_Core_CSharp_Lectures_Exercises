using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configurator.connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().Property(x => x.Email).IsUnicode(false);

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(x => new { x.PatientId, x.MedicamentId });
            });

            modelBuilder.Entity<Visitation>(entity =>
            {
                
                //entity.HasOne(x => x.Doctor).WithMany(x => x.Visitations).HasForeignKey(x => x.DoctorId)
                //.OnDelete(DeleteBehavior.Restrict);

                //entity.HasOne(x => x.Patient).WithMany(x => x.Visitations).HasForeignKey(x => x.PatientId)
                //.OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
