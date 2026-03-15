using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Entities
{
    public enum Gender { Male, Female }
    public abstract class User : BaseEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string ProfileImage { get; set; }
    }
}
