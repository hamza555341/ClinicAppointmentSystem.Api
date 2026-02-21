using ClinicAppointment.Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.Specifications
{
    internal class AppointmentWithDoctorSpecification
     : BaseSpecification<Appointment, int>
    {
        public AppointmentWithDoctorSpecification(int PatientId)
            :base(a=> a.PatientId==PatientId)
        {
            AddInclude(a => a.Doctor);
            AddInclude(a => a.Doctor.Specialization);

        }


        public AppointmentWithDoctorSpecification()
            : base(a => true)
        {
            AddInclude(a => a.Doctor);
            AddInclude(a => a.Doctor.Specialization);
        }
    }

}
