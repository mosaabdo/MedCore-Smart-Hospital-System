using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Entities
{
    public enum AppointmentStatus { Pending, Confirmed, Cancelled, Completed }

    public class Appointment : BaseEntity
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ScheduleId { get; set; }

        public AppointmentStatus Status { get; set; }
        public string? CancellationReason { get; set; } 

        // Navigation Properties
        public Patient Patient { get; set; }

        // One-to-One:
        public DoctorSchedule Schedule { get; set; }

        // Many-to-Many 
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
