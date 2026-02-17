using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Shared.DTOs.AppointmentDtos
{
    public class CreateAppointmentDTO
    {
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }

    }
}
