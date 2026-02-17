using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Shared.DTOs.IdentityDtos
{
    public record RegisterDoctorDTO(string DisplayName,
        [EmailAddress] string Email,
        [Phone] string PhoneNumber, 
        string Password,
        string UserName,
        string Bio,
        int SpecializationId,
        decimal Fees);

}
