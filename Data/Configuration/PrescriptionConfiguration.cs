using MedCoreSmartHospitalSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Data.Configuration
{
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            // Composite Key: 
            builder.HasKey(p => new { p.AppointmentId, p.MedicationId });

            // Concurrency & Audit
            builder.Property(p => p.Version).IsRowVersion();
            builder.Property<DateTime>("CreatedAt").HasDefaultValueSql("GETDATE()");

            // Relationships
            builder.HasOne(p => p.Appointment)
                   .WithMany(a => a.Prescriptions)
                   .HasForeignKey(p => p.AppointmentId);

            builder.HasOne(p => p.Medication)
                   .WithMany(m => m.Prescriptions)
                   .HasForeignKey(p => p.MedicationId);
        }
    }
}
