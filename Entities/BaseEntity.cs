using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Entities
{
    public class BaseEntity
    {
        // Concurrency Rule: .IsRowVersion()
        public byte[] Version { get; set; }

        // Audit Rules
        // CreatedAt will be a Shadow Property (configured in Fluent API)
        public DateTime LastModified { get; set; }

        // Soft Delete Rule
        public bool IsDeleted { get; set; }
    }
}
