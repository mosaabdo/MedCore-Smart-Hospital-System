using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MedCoreSmartHospitalSystem.Entities
{
    public class Specialty : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        // Navigation Property
        public ICollection<Doctor> Doctors { get; set; }
    }
}
