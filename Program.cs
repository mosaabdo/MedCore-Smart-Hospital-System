using MedCoreSmartHospitalSystem.Data;
using MedCoreSmartHospitalSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MedCoreSmartHospitalSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var context = new MedCoreDbContext())
            {
                try
                {
                    #region 0. Database Initialization
                    Console.WriteLine("--- [STEP 0] Cleaning and Initializing Database ---");
                    // Ensure a fresh start to apply all Fluent API changes correctly
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    Console.WriteLine("Database is fresh and ready!\n");
                    #endregion

                    #region 1. Basic Seeding (Inheritance & Relations)
                    Console.WriteLine("--- [STEP 1] Testing Basic Seeding & Inheritance ---");
                    var heartSpec = new Specialty
                    {
                        Name = "Cardiology",
                        Description = "Heart Health & Surgery",
                        Image = "cardio.png"
                    };
                    context.Specialties.Add(heartSpec);
                    context.SaveChanges();

                    var mainDoctor = new Doctor
                    {
                        FullName = "Dr. Mousa Abdelbasset",
                        NationalId = "29001011234567",
                        DateOfBirth = new DateTime(1990, 1, 1),
                        Gender = Gender.Male,
                        ProfileImage = "mousa.png",
                        SpecialtyId = heartSpec.Id,
                        LicenseNumber = "LIC-999",
                        HireDate = DateTime.Now.AddYears(-1),
                        HourRate = 1200
                    };
                    context.Doctors.Add(mainDoctor);
                    context.SaveChanges();
                    Console.WriteLine($"Success: Doctor {mainDoctor.FullName} added under {heartSpec.Name}.\n");
                    #endregion

                    #region 2. Audit Trail Test (Shadow Properties)
                    Console.WriteLine("--- [STEP 2] Testing Audit Trail ---");
                    var auditTarget = context.Doctors.First();
                    // Accessing the hidden "CreatedAt" property
                    var createdAt = context.Entry(auditTarget).Property("CreatedAt").CurrentValue;
                    Console.WriteLine($"Result: 'CreatedAt' Shadow Property automatically set to: {createdAt}\n");
                    #endregion

                    #region 3. Soft Delete Test (Global Filters)
                    Console.WriteLine("--- [STEP 3] Testing Soft Delete ---");
                    var tempSpec = new Specialty { Name = "Temporary Spec", Description = "To be deleted", Image = "temp.png" };
                    context.Specialties.Add(tempSpec);
                    context.SaveChanges();

                    // Perform Soft Delete
                    tempSpec.IsDeleted = true;
                    context.SaveChanges();

                    var visible = context.Specialties.Any(s => s.Id == tempSpec.Id);
                    var hiddenButExists = context.Specialties.IgnoreQueryFilters()
                                          .Any(s => s.Id == tempSpec.Id && s.IsDeleted);

                    Console.WriteLine($"Hidden from standard app query? {!visible}");
                    Console.WriteLine($"Still exists physically in DB? {hiddenButExists}\n");
                    #endregion

                    #region 4. Concurrency Test (RowVersion)
                    Console.WriteLine("--- [STEP 4] Testing Concurrency Control ---");
                    var user1View = context.Doctors.First();
                    var user2View = context.Doctors.AsNoTracking().First(); // Simulates another user

                    // User 1 updates and saves
                    user1View.FullName = "Dr. Mousa (Updated by User 1)";
                    context.SaveChanges();
                    Console.WriteLine("User 1: Successfully saved changes.");

                    // Detach User 1's entity to prevent tracking conflict in this test
                    context.Entry(user1View).State = EntityState.Detached;

                    try
                    {
                        Console.WriteLine("User 2: Attempting update with outdated RowVersion...");
                        context.Entry(user2View).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Console.WriteLine("Success: Concurrency Conflict caught! RowVersion prevented data loss.\n");
                    }
                    #endregion

                    #region 5. Check Constraints Test
                    Console.WriteLine("--- [STEP 5] Testing DB Constraints ---");
                    try
                    {
                        var invalidSchedule = new DoctorSchedule
                        {
                            DoctorId = mainDoctor.Id,
                            StartTime = DateTime.Now.AddHours(10),
                            EndTime = DateTime.Now.AddHours(5), // Invalid: End before Start
                            IsBooked = false
                        };
                        context.DoctorSchedules.Add(invalidSchedule);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Success: Database rejected invalid range. Message: {ex.InnerException?.Message}\n");
                    }
                    #endregion

                    #region 6. Massive Hospital Data Seeding
                    Console.WriteLine("--- [STEP 6] Massive Data Seeding for All Tables ---");

                    // A. Specialties
                    var surgery = new Specialty { Name = "General Surgery", Description = "Emergency & Elective Surgery", Image = "surg.png" };
                    var neuro = new Specialty { Name = "Neurology", Description = "Brain and Nervous System", Image = "neuro.png" };
                    context.Specialties.AddRange(surgery, neuro);

                    // B. Doctors
                    var drSara = new Doctor
                    {
                        FullName = "Dr. Sara Hassan",
                        NationalId = "295010199999",
                        DateOfBirth = new DateTime(1995, 2, 2),
                        Gender = Gender.Female,
                        ProfileImage = "sara.jpg",
                        Specialty = neuro,
                        LicenseNumber = "LIC-777",
                        HireDate = DateTime.Now,
                        HourRate = 900
                    };
                    context.Doctors.Add(drSara);

                    // C. Patients
                    var patient = new Patient
                    {
                        FullName = "Patient Ahmed Ali",
                        NationalId = "300010188888",
                        DateOfBirth = new DateTime(2005, 10, 10),
                        Gender = Gender.Male,
                        ProfileImage = "ahmed.jpg",
                        BloodType = BloodType.AB_Positive,
                        Allergies = new Allergies { FoodAllergies = "Peanuts", DrugAllergies = "Penicillin" },
                        ChronicConditions = new ChronicConditions { Description = "None" }
                    };
                    context.Patients.Add(patient);
                    context.SaveChanges();

                    // D. Doctor Availability (Schedule)
                    var slot = new DoctorSchedule
                    {
                        DoctorId = drSara.Id,
                        StartTime = DateTime.Now.AddDays(2).AddHours(9),
                        EndTime = DateTime.Now.AddDays(2).AddHours(10),
                        IsBooked = true // Marking as booked for the next step
                    };
                    context.DoctorSchedules.Add(slot);
                    context.SaveChanges();

                    // E. Appointment (One-to-One with Schedule)
                    var appointment = new Appointment
                    {
                        PatientId = patient.Id,
                        ScheduleId = slot.Id,
                        Status = AppointmentStatus.Confirmed
                    };
                    context.Appointments.Add(appointment);
                    context.SaveChanges();

                    // F. Medications & Prescriptions (Many-to-Many)
                    var med1 = new Medication { Name = "Aspirin", GenericName = "Acetylsalicylic acid" };
                    var med2 = new Medication { Name = "Voltaren", GenericName = "Diclofenac" };
                    context.Medications.AddRange(med1, med2);
                    context.SaveChanges();

                    var prescription1 = new Prescription
                    {
                        AppointmentId = appointment.Id,
                        MedicationId = med1.Id,
                        Dosage = "100mg",
                        Frequency = "Once daily"
                    };
                    context.Prescriptions.Add(prescription1);
                    context.SaveChanges();

                    Console.WriteLine("Success: Massive seeding completed for all 8 tables!\n");
                    #endregion
                }
                catch (Exception globalEx)
                {
                    Console.WriteLine($"CRITICAL ERROR: {globalEx.Message}");
                    if (globalEx.InnerException != null)
                        Console.WriteLine($"Inner Exception: {globalEx.InnerException.Message}");
                }
            }

            Console.WriteLine("==============================================");
            Console.WriteLine("All hospital system tests finished. Check your SQL Server!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}