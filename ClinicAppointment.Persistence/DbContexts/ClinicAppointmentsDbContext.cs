using ClinicAppointment.Domain.Entites.DoctorModule;
using ClinicAppointment.Domain.Entites.PatientModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Persistence.DbContexts
{
    public class ClinicAppointmentsDbContext : DbContext
    {

        public ClinicAppointmentsDbContext(DbContextOptions<ClinicAppointmentsDbContext> Options)
            : base(Options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }


    }
}
