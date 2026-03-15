using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Entities
{
    public class Medication : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GenericName { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
