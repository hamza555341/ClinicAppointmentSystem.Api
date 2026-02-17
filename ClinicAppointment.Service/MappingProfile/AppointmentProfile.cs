using AutoMapper;
using ClinicAppointment.Domain.Entites.AppointmentModule;
using ClinicAppointment.Shared.DTOs.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.MappingProfile
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<CreateAppointmentDTO, Appointment>();

               CreateMap<Appointment, AppointmentDTO>()
                    .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                    .ForMember(dest => dest.SpecializationName, opt => opt.MapFrom(src => src.Doctor.Specialization.Name)); 

        }



    }
}
