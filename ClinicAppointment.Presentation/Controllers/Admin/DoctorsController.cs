using ClinicAppointment.Service.Abstraction;
using ClinicAppointment.Shared.Common_Result;
using ClinicAppointment.Shared.DTOs.DoctorDtos;
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
    public class AdminDoctorsController : ApiBaseController
    {
        private readonly IDoctorService _doctorService;

        public AdminDoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // Get : BaseUrl/api/AdminDoctors

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return HandleResult(doctors);

        }

        // Get : BaseUrl/api/AdminDoctors/id

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDTO>> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetDoctorbyIdAsync(id);
 
            return HandleResult(doctor);
        }

        // Post : BaseUrl/api/AdminDoctors

        [HttpPost]
        public async Task<ActionResult<DoctorDTO>> CreateDoctor(CreateDoctorDto dto)
        {
            var createdDoctor = await _doctorService.CreateDoctorAsync(dto);

            if (createdDoctor.IsFailure)
                return HandleResult(createdDoctor);  

            var doctor = createdDoctor.Value;

            return CreatedAtAction(nameof(GetDoctorById),
                new { id = doctor.Id },
                doctor);
        }


        // Put : BaseUrl/api/AdminDoctors/id

        [HttpPut("{id}")]
        public async Task<ActionResult<DoctorDTO>> UpdateDoctor(int id, UpdateDoctorDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "Id mismatch" });

            var updatedDoctor = await _doctorService.UpdateDoctorAsync(dto);
            return HandleResult(updatedDoctor);
        }


        // Put : BaseUrl/api/AdminDoctors/id/Deactivate

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateDoctor(int id)
        {
            var result = await _doctorService.DeactivateDoctorAsync(id);

            return HandleResult(result);

        }

        // Put : BaseUrl/api/AdminDoctors/id/activate

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateDoctor(int id)
        {
            var result = await _doctorService.ActivateDoctorAsync(id);

            return HandleResult(result);
        }


    }
}
