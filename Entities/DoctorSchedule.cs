using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Entities
{
    public class DoctorSchedule : BaseEntity
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsBooked { get; set; } 

        // Navigation Properties
        public Doctor Doctor { get; set; }
        public Appointment Appointment { get; set; }
    }
}
