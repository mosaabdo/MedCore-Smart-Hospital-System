using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Entities
{
    public class Doctor : User
    {
        public int SpecialtyId { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime HireDate { get; set; }
        public decimal HourRate { get; set; }

        // Navigation Properties
        public Specialty Specialty { get; set; }

        // Appointment One-to-One with Schedule (will be linked later)
        public ICollection<DoctorSchedule> Schedules { get; set; }
    }
}
