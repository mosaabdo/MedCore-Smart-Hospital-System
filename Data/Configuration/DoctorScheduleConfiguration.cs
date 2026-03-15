using MedCoreSmartHospitalSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Data.Configuration
{
    public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            builder.HasKey(ds => ds.Id);

            // Check Constraint: EndTime > StartTime
            builder.ToTable(t => t.HasCheckConstraint("CH_Schedule_Time", "[EndTime] > [StartTime]"));

            // Global Rules (Concurrency & Audit)
            builder.Property(ds => ds.Version).IsRowVersion();
            builder.Property<DateTime>("CreatedAt").HasDefaultValueSql("GETDATE()");
            builder.HasQueryFilter(ds => !ds.IsDeleted);

            // Relationship: Doctor (1) <-> Schedules (Many)
            builder.HasOne(ds => ds.Doctor)
                   .WithMany(d => d.Schedules)
                   .HasForeignKey(ds => ds.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
