using ClinicAppointment.Service.Abstraction;
using ClinicAppointment.Shared.DTOs.SpecializationDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Presentation.Controllers.Patient
{

    [Authorize(Roles = "Patient")]

    public class PatientSpecializationsController : ApiBaseController
       {
        private readonly ISpecializationService _specializationService;

        public PatientSpecializationsController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        // Get: BaseUrl/api/PatientSpecializations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecializationDTO>>> GetAllSpecialization()
        {
            var specializations = await _specializationService.GetAllSpecializationsAsync();

            return HandleResult(specializations);
        }


        // Get: BaseUrl/api/PatientSpecializations/Id
        [HttpGet("{Id}")]
        public async Task<ActionResult<SpecializationDTO>> GetSpecializationById(int Id)
        {
            var specialization = await _specializationService.GetSpecializationByIdAsync(Id);      
            return HandleResult(specialization);


        }
    }
}
