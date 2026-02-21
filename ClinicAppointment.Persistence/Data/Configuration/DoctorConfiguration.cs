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
    internal class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {


            builder.Property(d => d.Id)
                   .UseIdentityColumn();

            builder.Property(d=> d.Name)
                .HasColumnType("nvarchar") 
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Bio)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(d => d.PictureUrl)
                .HasColumnType("nvarchar")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.ConsultationFees)
                .HasColumnType("decimal(18,2)");


            builder.HasOne(d => d.Specialization)
                .WithMany()
                .HasForeignKey(d => d.SpecializationId);

            builder.HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId);    

            builder.HasQueryFilter(d => d.IsActive);

            builder.Property(d => d.IsActive)
                .HasDefaultValue(true);


        }
    }
}
