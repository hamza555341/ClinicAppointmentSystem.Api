using ClinicAppointment.Shared.Common_Result;
using ClinicAppointment.Shared.DTOs.DoctorDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.Abstraction
{
    public interface IDoctorService
    {
        Task<Result> ActivateDoctorAsync(int doctorId);
        Task<Result> DeactivateDoctorAsync(int doctorId);
        Task<Result<IEnumerable<DoctorDTO>>> GetAllDoctorsAsync();  
        Task<Result<DoctorDTO>> GetDoctorbyIdAsync(int Id);
        Task<Result<DoctorDTO>> CreateDoctorAsync(CreateDoctorDto CdoctorDto);
        Task<Result<DoctorDTO>> UpdateDoctorAsync(UpdateDoctorDto UdoctorDto);
    }
}
