using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Entities
{
    public enum BloodType { A_Positive, A_Negative, B_Positive, B_Negative, AB_Positive, AB_Negative, O_Positive, O_Negative }
    public class Patient : User
    {
        public BloodType BloodType { get; set; }

        // Owned Types
        public Allergies Allergies { get; set; }
        public ChronicConditions ChronicConditions { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }

}
