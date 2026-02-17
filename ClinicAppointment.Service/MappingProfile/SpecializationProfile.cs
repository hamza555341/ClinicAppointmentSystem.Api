using AutoMapper;
using ClinicAppointment.Domain.Entites.DoctorModule;
using ClinicAppointment.Shared.DTOs.SpecializationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.MappingProfile
{
    public class SpecializationProfile:Profile
    {
        public SpecializationProfile()
        {
            CreateMap<Specialization, SpecializationDTO>();
            CreateMap<CreateSpecializationDTO, Specialization>();   
            CreateMap<UpdateSpecializationDTO, Specialization>();





        }





    }
}
