using MedCoreSmartHospitalSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Data.Configuration
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Version).IsRowVersion();
            builder.Property<DateTime>("CreatedAt").HasDefaultValueSql("GETDATE()");
            builder.HasQueryFilter(a => !a.IsDeleted);

            // One-to-One Relationship (The Core Rule)
            builder.HasOne(a => a.Schedule)
                   .WithOne(s => s.Appointment)
                   .HasForeignKey<Appointment>(a => a.ScheduleId)
                   .OnDelete(DeleteBehavior.Restrict);
            // Relationship with Patient
            builder.HasOne(a => a.Patient)
                   .WithMany(p => p.Appointments)
                   .HasForeignKey(a => a.PatientId);
            // Enum conversion 
            builder.Property(a => a.Status)
                   .HasConversion<string>();
        }
    }
}
