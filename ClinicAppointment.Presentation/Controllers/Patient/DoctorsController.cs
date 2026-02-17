using ClinicAppointment.Service.Abstraction;
using ClinicAppointment.Shared.DTOs.DoctorDtos;
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
    public class PatientDoctorsController:ApiBaseController
    {


        private readonly IDoctorService _doctorService;
        public PatientDoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }


        // Get : BaseUrl/api/PatientDoctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return HandleResult(doctors);

        }

        // Get : BaseUrl/api/PatientDoctors/id
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDTO>> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetDoctorbyIdAsync(id);
        
            return HandleResult(doctor);
        }


    }
}
