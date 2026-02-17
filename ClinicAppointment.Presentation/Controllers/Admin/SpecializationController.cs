using ClinicAppointment.Service.Abstraction;
using ClinicAppointment.Shared.DTOs.SpecializationDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Presentation.Controllers.Admin
{
    [Authorize(Roles = "Admin")]

    public class AdminSpecializationsController:ApiBaseController
    {
        private readonly ISpecializationService _specializationService;

        public AdminSpecializationsController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }


        // Post: BaseUrl/api/AdminSpecializations

        [HttpPost]
        public async Task<ActionResult<SpecializationDTO>> Create(CreateSpecializationDTO dto)
        {
            var CreatedSpec = await _specializationService.CreateSpecializationAsync(dto);
              
            return HandleResult(CreatedSpec);   

        }

        // Put: BaseUrl/api/AdminSpecializations/id
        [HttpPut("{id}")]
        public async Task<ActionResult<SpecializationDTO>> Update([FromRoute]int id, UpdateSpecializationDTO dto)

        {

              if (id != dto.Id)
                    return BadRequest(new { message = "Id mismatch" });
            
           var UpdatedSpecialization=await _specializationService.UpdateSpecializationAsync(dto);
            return HandleResult(UpdatedSpecialization);
        }

        // Delete: BaseUrl/api/AdminSpecializations/id

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete([FromRoute]int id)
            => Ok(await _specializationService.DeleteSpecializationAsync(id));




    }
}
