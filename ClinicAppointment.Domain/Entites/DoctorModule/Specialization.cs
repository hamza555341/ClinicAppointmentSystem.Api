using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Domain.Entites.DoctorModule
{
    public class Specialization :BaseEntity<int>    
    {
        public string Name { get; set; } = default!;    
    }
}
