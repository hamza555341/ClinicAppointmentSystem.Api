using AutoMapper;
using ClinicAppointment.Domain.Entites.DoctorModule;
using ClinicAppointment.Shared.DTOs.DoctorDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.MappingProfile
{
    public class DoctorProfile :Profile
    {
        public DoctorProfile()
        {
         CreateMap<Doctor, DoctorDTO>()
          .ForMember(dest=>dest.SpecializationName,opt=> opt.MapFrom(src=>src.Specialization.Name));
           
            CreateMap<CreateDoctorDto, Doctor>();
            CreateMap<UpdateDoctorDto, Doctor>();

        }




    }
}
