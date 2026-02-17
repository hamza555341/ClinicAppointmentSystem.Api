using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Shared.DTOs.AppointmentDtos
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = default!; 
        
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }=default!;
        public string SpecializationName { get; set; }=default!;    


    }
}
