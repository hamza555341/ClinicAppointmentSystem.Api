using ClinicAppointment.Domain.Entites.DoctorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.Specifications
{
    internal class DoctorWithSpecializationAndAppointmentSpecification :BaseSpecification<Doctor,int> 
    {

        // Get Doctor with Id And Include Specialization
        public DoctorWithSpecializationAndAppointmentSpecification(int id):
            base(D=>D.Id==id&& D.IsActive==true)
        {
            AddInclude(D => D.Specialization);
            AddInclude(D => D.Appointments);

        }

        // Get All Doctors with Include Specialization
        public DoctorWithSpecializationAndAppointmentSpecification():base(D=> D.IsActive == true)
        {
            AddInclude(D => D.Specialization);
            AddInclude(D => D.Appointments);
        }



    }
}
