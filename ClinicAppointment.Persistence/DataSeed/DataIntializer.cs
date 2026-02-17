using ClinicAppointment.Domain.Entites;
using ClinicAppointment.Domain.Entites.DoctorModule;
using ClinicAppointment.Domain.Interfaces;
using ClinicAppointment.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Persistence.DataSeed
{
    public class DataIntializer : IDataIntializer
    {
        private readonly ClinicAppointmentsDbContext _dbContext;

        public DataIntializer(ClinicAppointmentsDbContext DbContext)
        {
           _dbContext = DbContext;
         
        }


        public async Task InitializeDataAsync()
        {
            var hasDoctors = await _dbContext.Doctors.AnyAsync();
            var hasSpecializations = await _dbContext.Specializations.AnyAsync();

            if (hasDoctors && hasSpecializations) return;

             if (!hasSpecializations)
              //  await _dbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Specialization', RESEED, 0)");
                await SeedDataFromjson<Specialization,int>("Specializations.json", _dbContext.Specializations);
                await _dbContext.SaveChangesAsync();

            if (!hasDoctors)
            {
              //  await _dbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Doctors', RESEED, 0)");
                await SeedDataFromjson<Doctor,int>("Doctors.json", _dbContext.Doctors);
                await _dbContext.SaveChangesAsync();
            }
        }


        private async Task SeedDataFromjson<T,Tkey>(string FileName, DbSet<T> dbset)
            where T : BaseEntity<Tkey>
        {

   // E:\Clinic Appointment System\ClinicAppointmentSystem\ClinicAppointment.Persistence\DataSeed\JsonFiles\Doctors.json
          var FilePath = @"..\\ClinicAppointment.Persistence\DataSeed\JsonFiles\\" + FileName;
            if (!File.Exists(FilePath)) throw new FileNotFoundException($"File{FileName} is not Found");
            try
            {
             using var DataStream = File.OpenRead(FilePath);

              var DataList = await System.Text.Json.JsonSerializer.DeserializeAsync<List<T>>(DataStream,new System.Text.Json.JsonSerializerOptions()
              {
                  PropertyNameCaseInsensitive = true
              });

                if (DataList != null && DataList.Any())
                {
                    await dbset.AddRangeAsync(DataList);
                    await _dbContext.SaveChangesAsync();
                }


            }


            catch (Exception ex)
            {
                // هنا بنجيب الرسالة الداخلية اللي فيها تفاصيل رفض الداتابيز للبيانات
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception($"Database Error: {message}");
            }




        }

    }
}
