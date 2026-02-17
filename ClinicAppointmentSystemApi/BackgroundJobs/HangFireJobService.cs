using ClinicAppointment.Service.Abstraction;
using Hangfire;

namespace ClinicAppointmentSystemApi.BackgroundJobs
{
    public class HangfireJobService : IBackgroundJobService
    {
        public void ScheduleAppointmentCompletion(int appointmentId, DateTime runAt)
        {
            BackgroundJob.Schedule<IAppointmentService>(
                service => service.AutoCompleteAppointmentAsync(appointmentId),
                runAt);
        }
    }
}
