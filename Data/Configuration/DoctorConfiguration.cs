using MedCoreSmartHospitalSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Data.Configuration
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            // License Number is usually unique and required
            builder.Property(d => d.LicenseNumber).IsRequired().HasMaxLength(50);

            // HourRate decimal precision (Best practice for money)
            builder.Property(d => d.HourRate).HasPrecision(18, 2);

            // One-to-Many Relationship: Specialty -> Doctors
            builder.HasOne(d => d.Specialty)
                   .WithMany(s => s.Doctors)
                   .HasForeignKey(d => d.SpecialtyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
