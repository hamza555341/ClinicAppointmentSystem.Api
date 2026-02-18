using ClinicAppointment.Domain.Entites.DoctorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.Specifications
{
    internal class DoctorWithSpecializationSpecification :BaseSpecification<Doctor,int> 
    {

        // Get Doctor with Id And Include Specialization
        public DoctorWithSpecializationSpecification(int id):base(D=>D.Id==id)
        {
            AddInclude(D => D.Specialization);

        }

        // Get All Doctors with Include Specialization
        public DoctorWithSpecializationSpecification():base(null!)
        {
            AddInclude(D => D.Specialization);
        }



    }
}
