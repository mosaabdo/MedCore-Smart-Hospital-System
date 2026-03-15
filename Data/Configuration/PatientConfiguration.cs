using MedCoreSmartHospitalSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Data.Configuration
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.OwnsOne(p => p.Allergies);
            builder.OwnsOne(p => p.ChronicConditions);

            builder.Property(p => p.BloodType).HasConversion<string>();
        }
    }
}
