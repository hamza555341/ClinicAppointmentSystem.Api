using ClinicAppointment.Shared.Common_Result;
using ClinicAppointment.Shared.DTOs.SpecializationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.Abstraction
{
    public interface ISpecializationService
    {
        // Patient side (read only)
        Task<Result<IEnumerable<SpecializationDTO>>> GetAllSpecializationsAsync();
        Task<Result<SpecializationDTO>> GetSpecializationByIdAsync(int id);

        // Admin side (CRUD)
        Task<Result<SpecializationDTO>> CreateSpecializationAsync(CreateSpecializationDTO Cdto);
        Task<Result<SpecializationDTO>> UpdateSpecializationAsync(UpdateSpecializationDTO Udto);
        Task<bool> DeleteSpecializationAsync(int id);




    }
}
