using AutoMapper;
using ClinicAppointment.Domain.Entites.AppointmentModule;
using ClinicAppointment.Domain.Entites.DoctorModule;
using ClinicAppointment.Domain.Interfaces;
using ClinicAppointment.Service.Abstraction;
using ClinicAppointment.Service.Specifications;
using ClinicAppointment.Shared.Common_Result;
using ClinicAppointment.Shared.DTOs.DoctorDtos;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service
{


    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        public async Task<Result<DoctorDTO>> CreateDoctorAsync(CreateDoctorDto CdoctorDto)
        {        
                var SpecializationExist = await _unitOfWork.GetRepository<Specialization, int>()
                                              .GetByIdAsync(CdoctorDto.SpecializationId);
            if (SpecializationExist is null)
              return Error.NotFound("Specialization.NotFound", $"Specialization With{CdoctorDto.SpecializationId} Is Not Found");

            if (CdoctorDto.ConsultationFees <= 0)
                return Error.Validation("ConsFees.Validation", $"ConsultationFees With {CdoctorDto.ConsultationFees} is less than 1");

            var doctor = _mapper.Map<Doctor>(CdoctorDto);

                doctor.Specialization = SpecializationExist;

              await _unitOfWork.GetRepository<Doctor, int>().AddAsync(doctor);
              bool IsCreated =  await _unitOfWork.SaveChangesAsync() > 0;
                if (!IsCreated)
                return Error.Failure("CreateDoctor.failed");

            return Result<DoctorDTO>.Ok(_mapper.Map<DoctorDTO>(doctor));
                            
        }

        public async Task<Result> DeactivateDoctorAsync(int doctorId)
        {
            var doctorRepo = _unitOfWork.GetRepository<Doctor, int>();

            var doctor = await doctorRepo.GetByIdAsync(doctorId);

            if (doctor is null)
                return Result.Failure(Error.NotFound("Doctor.NotFound",
                    $"Doctor with id {doctorId} not found"));

            if (!doctor.IsActive)
                return Result.Failure(Error.Validation("Doctor.AlreadyInactive",
                    "Doctor is already inactive"));

            doctor.IsActive = false;

            doctorRepo.Update(doctor);

            var saved = await _unitOfWork.SaveChangesAsync() > 0;

            if (!saved)
                return Result.Failure( Error.Failure("Doctor.DeactivateFailed"));

            return Result.Ok();
        }

        public async Task<Result> ActivateDoctorAsync(int doctorId)
        {
            var doctorRepo = _unitOfWork.GetRepository<Doctor, int>();

            var doctor = await doctorRepo.GetByIdAsync(doctorId);

            if (doctor is null)
                return Result.Failure(Error.NotFound("Doctor.NotFound"));

            if (doctor.IsActive)
                return Result.Failure(Error.Validation("Doctor.AlreadyActive"));

            doctor.IsActive = true;

            doctorRepo.Update(doctor);

            var saved = await _unitOfWork.SaveChangesAsync() > 0;

            if (!saved)
                return Result.Failure(Error.Failure("Doctor.ActivateFailed"));

            return Result.Ok();
        }

        public async Task<Result<IEnumerable<DoctorDTO>>> GetAllDoctorsAsync()
        {
            var repo = _unitOfWork.GetRepository<Doctor, int>();
            var Spec = new DoctorWithSpecializationAndAppointmentSpecification();
            var Doctors = await repo.GetAllAsync(Spec);

            return Result<IEnumerable<DoctorDTO>>.Ok(_mapper.Map<IEnumerable<DoctorDTO>>(Doctors));
        }

        public async Task<Result<DoctorDTO>> GetDoctorbyIdAsync(int Id)
        {
            var repo = _unitOfWork.GetRepository<Doctor, int>();
            var Spec = new DoctorWithSpecializationAndAppointmentSpecification(Id);
            var Doctor = await repo.GetByIdAsync(Spec);
            if (Doctor is null)
                return Error.NotFound("Doctor.NotFound", $"Doctor With{Id} Is Not Found");

            return Result<DoctorDTO>.Ok(_mapper.Map<DoctorDTO>(Doctor));  

        }

        public async Task<Result<DoctorDTO>> UpdateDoctorAsync(UpdateDoctorDto UdoctorDto)
        {
          var doctorRepo =  _unitOfWork.GetRepository<Doctor, int>();
            var specializationRepo = _unitOfWork.GetRepository<Specialization, int>();


            //  check if doctor exists
            var doctor = await doctorRepo.GetByIdAsync(UdoctorDto.Id);
            if (doctor is null)
                return Result<DoctorDTO>.Failure(Error.NotFound("Doctor.NotFound", $"Doctor With {UdoctorDto.Id} is Not Found"));

            // check if specialization exists
           var Specialization = await specializationRepo.GetByIdAsync(UdoctorDto.SpecializationId);
            if (Specialization == null)
                return Result<DoctorDTO>.Failure(Error.NotFound("Specialization.NotFound", $"Specialization With {UdoctorDto.SpecializationId} is Not Found"));
            //validate consultation fees

            if (UdoctorDto.ConsultationFees <= 0)
                return Result<DoctorDTO>.Failure(Error.Validation("ConsFees.Validation", $"ConsultationFees With {UdoctorDto.ConsultationFees} is less than 1"));
            // map the updated fields

            _mapper.Map(UdoctorDto, doctor);    

             doctor.Specialization = Specialization;
            // save changes
            var IsUpdated = await _unitOfWork.SaveChangesAsync() > 0;

            if (!IsUpdated)
                return Result<DoctorDTO>.Failure(Error.InValidCerdentials());

            return Result<DoctorDTO>.Ok(_mapper.Map<DoctorDTO>(doctor));

        }

    }
}
