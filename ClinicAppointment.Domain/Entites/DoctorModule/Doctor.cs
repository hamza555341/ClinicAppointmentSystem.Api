using ClinicAppointment.Domain.Entites.AppointmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Domain.Entites.DoctorModule
{
    public class Doctor : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Bio { get; set; } = default!;
        public string? PictureUrl { get; set; }
        public decimal ConsultationFees { get; set; } = default!;


        public string? IdentityUserId { get; set; }

        #region Relations
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; } = default!;

        public ICollection<Appointment> Appointments { get; set; } = [];
        #endregion

    }
}
