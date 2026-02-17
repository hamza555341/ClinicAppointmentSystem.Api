using AutoMapper;
using ClinicAppointment.Domain.Entites.AppointmentModule;
using ClinicAppointment.Domain.Entites.DoctorModule;
using ClinicAppointment.Domain.Entites.PatientModule;
using ClinicAppointment.Domain.Interfaces;
using ClinicAppointment.Service.Abstraction;
using ClinicAppointment.Shared.Common_Result;
using ClinicAppointment.Shared.DTOs.AppointmentDtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobService _backgroundJobService;

        public AppointmentService(IUnitOfWork unitOfWork,IMapper mapper, IBackgroundJobService backgroundJobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _backgroundJobService = backgroundJobService;
        }


        public async Task<Result<AppointmentDTO>> BookAppointmentAsync(string UserId, CreateAppointmentDTO dto)
        {

            var patientRepo =  _unitOfWork.GetRepository<Patient, int>();

            var patient = (await patientRepo.GetAllAsync(p=>p.IdentityUserId==UserId))
                          .FirstOrDefault();


            if (patient is null)
                return Error.NotFound("Patient.NotFound");     
            
            var doctorRepo=  _unitOfWork.GetRepository<Doctor,int>();    
            var appointmentRepo= _unitOfWork.GetRepository<Appointment,int>();

          var doctor= (await doctorRepo.GetAllAsync(d => d.Id == dto.DoctorId, d => d.Specialization,d=>d.Appointments))
                        .FirstOrDefault();



            var cairoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

            // اعتبر الوقت الجاي من المستخدم بتوقيت مصر
            var localAppointmentTime = DateTime.SpecifyKind(dto.AppointmentDate, DateTimeKind.Unspecified);

            // الوقت الحالي في مصر
            var nowInCairo = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cairoTimeZone);

            if (localAppointmentTime <= nowInCairo)
            {
                return Error.Validation("Invalid appointment date", "Appointment date must be in the future");
            }

            // بعد ما نتأكد إنه في المستقبل — نحوله UTC للتخزين
            var utcDate = TimeZoneInfo.ConvertTimeToUtc(localAppointmentTime, cairoTimeZone);


            // check if doctor exist
            if (doctor is null)
            {
              return Error.NotFound("Doctor not found",$"doctor With{dto.DoctorId} is not found");
            }      

            // check if doctor is available at the requested appointment date
            if (doctor.Appointments.Any(a=>a.AppointmentDate== utcDate &&
            a.Status != AppointmentStatus.Cancelled))
            {
                return Error.Validation("Appointment.TimeSlotTaken",
                           $"Doctor is not available at {dto.AppointmentDate}");
            }

          var appointmentToStore= _mapper.Map<Appointment>(dto);
            appointmentToStore.PatientId = patient.Id ;
            appointmentToStore.Status = AppointmentStatus.Pending;
            appointmentToStore.AppointmentDate = utcDate;


            await appointmentRepo.AddAsync(appointmentToStore);
             var IsCreated= await _unitOfWork.SaveChangesAsync()>0;

            if (!IsCreated)
              return Error.Failure("Failed to book appointment",$"An error occurred while booking the appointment");




            //  Schedule auto-complete after appointment time
            _backgroundJobService.ScheduleAppointmentCompletion(
                appointmentToStore.Id,
                utcDate.AddMinutes(30));


            var appointmentDto= _mapper.Map<AppointmentDTO>(appointmentToStore);
            appointmentDto.AppointmentDate =
            TimeZoneInfo.ConvertTimeFromUtc(appointmentToStore.AppointmentDate, cairoTimeZone);

            return Result<AppointmentDTO>.Ok(appointmentDto);   

        }

        public async Task<Result> CancelAppointmentAsync(int AppointmentId, string UserId)
        {

            var patientRepo = _unitOfWork.GetRepository<Patient, int>();

            var patient = (await patientRepo.GetAllAsync(p => p.IdentityUserId == UserId))
                          .FirstOrDefault();



            var Appointment= await _unitOfWork.GetRepository<Appointment,int>()
                                       .GetByIdAsync(AppointmentId);

            // check if appointment exist
              if (Appointment is null)
              {
                return Result.Failure( Error.NotFound("Appointment not found",$"Appointment with id {AppointmentId} is not found"));
              }

            // check if the appointment belongs to the patient
             if (Appointment.PatientId != patient!.Id)
             {
               return Result.Failure( Error.Forbidden("Unauthorized",$"You are not authorized to cancel this appointment"));
             }

            //check if appointment date is in the past
            if (Appointment.AppointmentDate <= DateTime.UtcNow)
            {
                return Result.Failure( Error.Validation("Invalid appointment date",$"You cannot cancel an appointment that has already occurred"));
            }

                Appointment.Status = AppointmentStatus.Cancelled;
                _unitOfWork.GetRepository<Appointment,int>().Update(Appointment);
                var IsCancelled= await _unitOfWork.SaveChangesAsync()>0;
            if (!IsCancelled)
            {
                return Result<AppointmentDTO>.Failure( Error.Failure("Failed to cancel appointment",$"An error occurred while cancelling the appointment"));
            }

            return Result.Ok();


        }

        public async Task<Result> ChangeAppointmentStatusAsync(ChangeAppointmentStatusDTO dto)
        {
          var appointmentRepo= _unitOfWork.GetRepository<Appointment,int>();    

            var appointment= await appointmentRepo.GetByIdAsync(dto.AppointmentId);
    
              if (appointment is null)
              {
                return Result.Failure(Error.NotFound("Appointment not found",$"Appointment with id {dto.AppointmentId} is not found"));
              }

          // prevent changing status of completed or cancelled appointments
            if (appointment.Status== AppointmentStatus.Completed
                  || appointment.Status == AppointmentStatus.Cancelled)
            {
                return Result.Failure(Error.Validation("Invalid appointment status change",$"You cannot change the status of a completed or cancelled appointment"));
            }

            // validate the status transition
            var valid = appointment.Status switch
            { 
                AppointmentStatus.Pending=> dto.NewStatus== AppointmentStatus.Confirmed
                || dto.NewStatus == AppointmentStatus.Cancelled,
                 
                AppointmentStatus.Confirmed=> dto.NewStatus==AppointmentStatus.Cancelled
                 || dto.NewStatus == AppointmentStatus.Completed,

               _ => false

            };


            if(!valid)
            {
                return Result.Failure(Error.
                 Validation("Invalid appointment status change",$"You cannot change the appointment status from {appointment.Status} to {dto.NewStatus}"));
            }   

            appointment.Status = dto.NewStatus;

            appointmentRepo.Update(appointment);

            var IsUpdated= await _unitOfWork.SaveChangesAsync()>0;
            if (!IsUpdated)
            {
                return Result.Failure(Error.
                    Failure("Failed to change appointment status",$"An error occurred while changing the appointment status"));
            }

            return Result.Ok(); 


        }

        public async Task AutoCompleteAppointmentAsync(int AppointmentId)
        {
          var appointmentRepo= _unitOfWork.GetRepository<Appointment,int>();  
            
           var appointment= await appointmentRepo.GetByIdAsync(AppointmentId);


            if(appointment is null)
            {
                return;
            }   

            // check Appointment status is pending or confirmed before marking it as completed
            if (appointment!.Status == AppointmentStatus.Pending
               || appointment.Status == AppointmentStatus.Confirmed)
            { 
              appointment.Status = AppointmentStatus.Completed;
                appointmentRepo.Update(appointment);
                await _unitOfWork.SaveChangesAsync();

            }

        }

        public async Task<Result<IEnumerable<AppointmentDTO>>> GetAllAppointmentAsync()
        {
            var Appointments = await _unitOfWork.GetRepository<Appointment, int>().GetAllAsync
                    ( null,A => A.Doctor, A => A.Doctor.Specialization);

            var ordered = Appointments.OrderByDescending(a => a.AppointmentDate).ToList();

            var AppointmentToReturn = _mapper.Map<IEnumerable<AppointmentDTO>>(Appointments);
            return Result<IEnumerable<AppointmentDTO>>.Ok(AppointmentToReturn);
        }

        public async Task<Result<IEnumerable<AppointmentDTO>>> GetPatientAppointmentsAsync(string UserId)
        {

            var patientRepo = _unitOfWork.GetRepository<Patient, int>();

            var patient = (await patientRepo.GetAllAsync(p => p.IdentityUserId == UserId))
                          .FirstOrDefault();


            var Appointments = await _unitOfWork.GetRepository<Appointment, int>().GetAllAsync
                   (A => A.PatientId == patient!.Id, A => A.Doctor, A => A.Doctor.Specialization);

            var ordered = Appointments.OrderByDescending(a => a.AppointmentDate).ToList();

            var AppointmentToReturn=  _mapper.Map<IEnumerable<AppointmentDTO>>(Appointments);
            return Result<IEnumerable<AppointmentDTO>>.Ok(AppointmentToReturn);

        }
    }
}
