using AutoMapper;
using ClinicAppointment.Domain.Entites.DoctorModule;
using ClinicAppointment.Domain.Interfaces;
using ClinicAppointment.Service.Abstraction;
using ClinicAppointment.Shared.Common_Result;
using ClinicAppointment.Shared.DTOs.SpecializationDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service
{
    public class SpecializationService : ISpecializationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SpecializationService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IMapper Mapper { get; }

        public async Task<Result<SpecializationDTO>> CreateSpecializationAsync(CreateSpecializationDTO Cdto)
        {
            var SpecRepo= _unitOfWork.GetRepository<Specialization,int>();  

            var Specialization= _mapper.Map<Specialization>(Cdto);

            await SpecRepo.AddAsync(Specialization);

         var IsCreated = await _unitOfWork.SaveChangesAsync()>0;

            if (!IsCreated)
            {
                return Error.Failure("CreatedASpec.failed");
            }

          return Result<SpecializationDTO>.Ok(_mapper.Map<SpecializationDTO>(Specialization));


        }

        public async Task<bool> DeleteSpecializationAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Specialization, int>();

            var specialization = await repo.GetByIdAsync(id);
            if (specialization == null)
                return false;

            repo.Delete(specialization);
          return  await _unitOfWork.SaveChangesAsync()>0 ;
        }

        public async Task<Result<IEnumerable<SpecializationDTO>>> GetAllSpecializationsAsync()
        {
            var specs = await _unitOfWork
              .GetRepository<Specialization, int>()
              .GetAllAsync();

            return Result<IEnumerable<SpecializationDTO>>.Ok(_mapper.Map<IEnumerable<SpecializationDTO>>(specs));
        }

        public async Task<Result<SpecializationDTO>> GetSpecializationByIdAsync(int id)
        {
            var spec = await _unitOfWork
             .GetRepository<Specialization, int>()
             .GetByIdAsync(id);

            if (spec is null)
                return Error.NotFound("Specialization.NotFound", $"Specialization With{id} Is Not Found");

            return Result<SpecializationDTO>.Ok(_mapper.Map<SpecializationDTO>(spec));
        }

        public async Task<Result<SpecializationDTO>> UpdateSpecializationAsync(UpdateSpecializationDTO Udto)
        {
            var repo = _unitOfWork.GetRepository<Specialization, int>();

            var specialization = await repo.GetByIdAsync(Udto.Id);
            if (specialization is null)
                return Error.NotFound("Specialization.NotFound", $"Specialization With{Udto.Id} Is Not Found");

            _mapper.Map(Udto, specialization);

            repo.Update(specialization);
            await _unitOfWork.SaveChangesAsync();

            return Result<SpecializationDTO>.Ok(_mapper.Map<SpecializationDTO>(specialization));
        }
    }
}
