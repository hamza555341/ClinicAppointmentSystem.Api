using ClinicAppointment.Domain.Entites.PatientModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.Specifications
{
    internal class PatientByIdentityUserIdSpecification:BaseSpecification<Patient,int>
    {

        public PatientByIdentityUserIdSpecification(string UserId)
            :base(p=>p.IdentityUserId==UserId)
        {
            
        }



    }
}
