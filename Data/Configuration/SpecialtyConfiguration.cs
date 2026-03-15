using MedCoreSmartHospitalSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Data.Configuration
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            // Primary Key
            builder.HasKey(s => s.Id);

            // Version Column for Concurrency (This is what caused the error)
            builder.Property(s => s.Version)
                   .IsRowVersion();

            // Audit Shadow Property (Required in your project rules)
            builder.Property<DateTime>("CreatedAt")
                   .HasDefaultValueSql("GETDATE()");

            // Global Query Filter for Soft Delete
            builder.HasQueryFilter(s => !s.IsDeleted);

            builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        }
    }
}
