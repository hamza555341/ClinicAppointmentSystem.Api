using ClinicAppointment.Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Domain.Entites.PatientModule
{
    public class Patient :BaseEntity<int>
    {
        public string FullName { get; set; }=default!;
        public string PhoneNumber { get; set; }=default!;
        public string IdentityUserId { get; set; }= default!;

        public ICollection<Appointment> Appointments { get; set; } = [];    

    }
}
