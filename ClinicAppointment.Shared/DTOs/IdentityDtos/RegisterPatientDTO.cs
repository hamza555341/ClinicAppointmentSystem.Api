using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Shared.DTOs.IdentityDtos
{
    public record RegisterPatientDTO(string DisplayName, [EmailAddress] string Email, [Phone] string PhoneNumber, string Password, string UserName);

}
