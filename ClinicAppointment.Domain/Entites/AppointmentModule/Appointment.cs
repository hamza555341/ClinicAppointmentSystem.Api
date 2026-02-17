using ClinicAppointment.Domain.Entites.DoctorModule;
using ClinicAppointment.Domain.Entites.PatientModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Domain.Entites.AppointmentModule
{
    public class Appointment : BaseEntity<int>
    {
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;


        #region Relations
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = default!;
        #endregion



    }
}
