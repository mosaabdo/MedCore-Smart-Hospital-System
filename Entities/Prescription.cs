using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Entities
{
    public class Prescription : BaseEntity
    {
        public int AppointmentId { get; set; }
        public int MedicationId { get; set; }

        public string Dosage { get; set; }
        public string Frequency { get; set; }

        // Navigation Properties
        public Appointment Appointment { get; set; }
        public Medication Medication { get; set; }
    }
}
