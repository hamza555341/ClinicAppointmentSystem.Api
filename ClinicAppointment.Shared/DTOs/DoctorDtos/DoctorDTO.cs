using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Shared.DTOs.DoctorDtos
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Bio { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal ConsultationFees { get; set; }
        public string SpecializationName { get; set; } = default!;
    }
}
