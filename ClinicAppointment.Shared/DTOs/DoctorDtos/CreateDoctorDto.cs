using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Shared.DTOs.DoctorDtos
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must Be Between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can Contain Only Character and Spaces")]
        public string Name { get; set; }=default!;

        public string Bio { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;

        public decimal ConsultationFees { get; set; }
        public int SpecializationId { get; set; }
    }
}
