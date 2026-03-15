using MedCoreSmartHospitalSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Data
{
    public class MedCoreDbContext : DbContext
    {
        public MedCoreDbContext(DbContextOptions<MedCoreDbContext> options) : base(options) { }
        public MedCoreDbContext() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlServer("Server=.;Database=MedCoreDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply any existing configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MedCoreDbContext).Assembly);

            // This loop ensures EVERY entity has the Shadow Property and Versioning correctly
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // 1. Add Shadow Property 'CreatedAt' to all entities inheriting from BaseEntity
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property<DateTime>("CreatedAt")
                        .HasDefaultValueSql("GETDATE()");

                    // 2. Ensure Version is handled as RowVersion for ALL entities
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("Version")
                        .IsRowVersion();
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {

                    case EntityState.Added:
                        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
