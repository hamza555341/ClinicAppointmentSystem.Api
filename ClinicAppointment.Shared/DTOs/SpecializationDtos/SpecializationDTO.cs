using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Shared.DTOs.SpecializationDtos
{
    public class SpecializationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }= default!;
    }
}
