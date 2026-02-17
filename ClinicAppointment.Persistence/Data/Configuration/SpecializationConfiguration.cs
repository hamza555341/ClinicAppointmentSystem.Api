using ClinicAppointment.Domain.Entites.AppointmentModule;
using ClinicAppointment.Domain.Entites.DoctorModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Persistence.Data.Configuration
{
    public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {

            builder.HasKey(s => s.Id);

            builder.Property(x => x.Id)
           .UseIdentityColumn(); // يخليها Identity في SQL Server



        }
    }
}
