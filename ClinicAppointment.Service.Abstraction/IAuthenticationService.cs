using ClinicAppointment.Shared.Common_Result;
using ClinicAppointment.Shared.DTOs.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.Abstraction
{
    public interface IAuthenticationService
    {
        // Login
        //Email ,Password => Token ,Email ,DisplayName
        Task<Result<UserDTO>> LoginAsyns(LoginDto loginDto);

       //Register Patient
       //Email, Password , UserName ,PhoneNumber ,DisplayName => Token ,Email ,DisplayName
        Task<Result<UserDTO>> RegisterPatientAsync(RegisterPatientDTO dto);

       //Register Doctor
      // Email, Password , UserName ,PhoneNumber ,DisplayName,Bio,Fees,SpecializationId => Token ,Email ,DisplayName
                        
        Task<Result<UserDTO>> RegisterDoctorAsync(RegisterDoctorDTO dto);
   
        Task<Result<CurrentUserDTO>> GetCurrentUserAsync(string userId);
        Task<bool> CheckEmailAsync(string email);

    }
}
