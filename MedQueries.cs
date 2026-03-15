using MedCoreSmartHospitalSystem.Data;
using MedCoreSmartHospitalSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem
{
    public static class MedQueries
    {
        // High-performance search for a doctor by National ID
        public static readonly Func<MedCoreDbContext, string, Doctor?> GetDoctorByNationalId =
            EF.CompileQuery((MedCoreDbContext context, string nationalId) =>
                context.Doctors.AsNoTracking().FirstOrDefault(d => d.NationalId == nationalId));

        // High-performance search for all medications for an appointment
        public static readonly Func<MedCoreDbContext, int, IEnumerable<Medication>> GetMedicationsByAppointment =
            EF.CompileQuery((MedCoreDbContext context, int appointmentId) =>
                context.Prescriptions
                    .AsNoTracking()
                    .Where(p => p.AppointmentId == appointmentId)
                    .Select(p => p.Medication));
    }
}
