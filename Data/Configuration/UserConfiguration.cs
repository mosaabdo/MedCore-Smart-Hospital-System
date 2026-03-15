using MedCoreSmartHospitalSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Data.Configuration
    {

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Unique Index on NationalId
            builder.HasIndex(u => u.NationalId).IsUnique();

            // Check Constraint: DOB < CurrentDate
            // SQL Server syntax: [DateOfBirth] < GETDATE()
            builder.ToTable(t => t.HasCheckConstraint("CH_User_DOB", "[DateOfBirth] < GETDATE()"));

            // Configure RowVersion for Concurrency
            builder.Property(u => u.Version).IsRowVersion();

            // Configure Shadow Property for Audit
            builder.Property<DateTime>("CreatedAt").HasDefaultValueSql("GETDATE()");

            // Global Query Filter for Soft Delete
            builder.HasQueryFilter(u => !u.IsDeleted);

            builder.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.NationalId).IsRequired().HasMaxLength(14);
        }
    }

}