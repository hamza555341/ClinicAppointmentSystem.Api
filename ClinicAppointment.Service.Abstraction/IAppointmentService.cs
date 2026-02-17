using ClinicAppointment.Domain.Entites.PatientModule;
using ClinicAppointment.Shared.Common_Result;
using ClinicAppointment.Shared.DTOs.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.Abstraction
{
    public interface IAppointmentService
    {
        Task<Result<AppointmentDTO>> BookAppointmentAsync(string UserId, CreateAppointmentDTO dto);
        Task<Result> CancelAppointmentAsync(int AppointmentId, string UserId); //for patient to cancel their appointment
        Task<Result<IEnumerable<AppointmentDTO>>> GetPatientAppointmentsAsync(string UserId);//for patient to view their appointments
        Task<Result<IEnumerable<AppointmentDTO>>> GetAllAppointmentAsync(); // For Admin
        Task<Result> ChangeAppointmentStatusAsync(ChangeAppointmentStatusDTO dto); // for Admin
        Task AutoCompleteAppointmentAsync(int AppointmentId); //For Hangfire job to change status to Completed after appointment time has passed
                                                            

    }
}
