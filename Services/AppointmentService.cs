using MedCoreSmartHospitalSystem.Data;
using MedCoreSmartHospitalSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedCoreSmartHospitalSystem.Services
{
    public class AppointmentService
    {
        private readonly MedCoreDbContext _context;
        public AppointmentService(MedCoreDbContext context)
        {
            _context = context;
        }

        // 1. Transactional Cancellation 
        public async Task CancelAppointmentAsync(int appointmentId, string reason)
        {
            // بنفتح Transaction عشان نضمن إن الخطوتين يحصلوا مع بعض
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Schedule)
                    .FirstOrDefaultAsync(a => a.Id == appointmentId);

                if (appointment == null) throw new Exception("Appointment not found");

                // الخطوة 1: تغيير حالة الموعد
                appointment.Status = AppointmentStatus.Cancelled;
                appointment.CancellationReason = reason;

                // الخطوة 2: فتح خانة الدكتور مرة أخرى (Atomicity)
                if (appointment.Schedule != null)
                {
                    appointment.Schedule.IsBooked = false;
                }

                await _context.SaveChangesAsync();

                // لو كل شيء تمام، بنعمل Commit
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // لو حصل أي خطأ في أي خطوة، بنرجع كل حاجة زي ما كانت
                await transaction.RollbackAsync();
                throw;
            }
        }

        // 2. Concurrency Booking (المطلب الخاص بالـ DbUpdateConcurrencyException)
        public async Task BookAppointmentAsync(int patientId, int scheduleId)
        {
            try
            {
                var schedule = await _context.DoctorSchedules.FindAsync(scheduleId);

                if (schedule == null || schedule.IsDeleted || schedule.IsBooked)
                    throw new Exception("Slot not available.");

                var newAppointment = new Appointment
                {
                    PatientId = patientId,
                    ScheduleId = scheduleId,
                    Status = AppointmentStatus.Confirmed
                };

                schedule.IsBooked = true;

                _context.Appointments.Add(newAppointment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // هنا السيستم بيمسك حالة لو اتنين داسوا حجز في نفس اللحظة
                // الـ RowVersion (byte[] Version) هو اللي بيخلي EF يكتشف ده
                Console.WriteLine("Concurrency Conflict: This slot was just booked by someone else.");

                // اقتراح الموعد التالي (كجزء من الركويرمنت)
                var nextSlot = await _context.DoctorSchedules
                    .Where(s => !s.IsBooked && !s.IsDeleted)
                    .OrderBy(s => s.StartTime)
                    .FirstOrDefaultAsync();

                if (nextSlot != null)
                {
                    Console.WriteLine($"Suggested next available slot: {nextSlot.StartTime}");
                }
            }
        }
    }
}
