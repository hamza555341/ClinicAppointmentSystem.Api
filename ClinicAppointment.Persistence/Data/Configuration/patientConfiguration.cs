using ClinicAppointment.Domain.Entites.PatientModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Persistence.Data.Configuration
{
    public class patientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
          builder.Property(p => p.FullName)
                 .IsRequired()
                 .HasColumnType("nvarchar")
                 .HasMaxLength(100);

         builder.Property(p => p.PhoneNumber)
                 .IsRequired()
                 .HasColumnType("nvarchar")
                 .HasMaxLength(15);


            builder.HasMany(p => p.Appointments)
                   .WithOne(a => a.Patient)
                   .HasForeignKey(a => a.PatientId);
                   


        }
    }
}
