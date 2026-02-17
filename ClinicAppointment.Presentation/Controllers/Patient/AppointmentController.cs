using ClinicAppointment.Service.Abstraction;
using ClinicAppointment.Shared.DTOs.AppointmentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ClinicAppointment.Presentation.Controllers.Patient
{

    [Authorize(Roles = "Patient")]

    public class PatientAppointmentController: ApiBaseController
    {
        private readonly IAppointmentService _appointmentService;

        public PatientAppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        //POST BaseUrl/api/PatientAppointment
        [HttpPost]
        public async Task<ActionResult<AppointmentDTO>> BookAppointment(CreateAppointmentDTO dto)
        {
          // to Confirm that User Id Which Fitched Not User Name or anything From Token
            var UserId = User.Claims.FirstOrDefault(c => c.Value.Length > 20)?.Value;

            var result= await _appointmentService.BookAppointmentAsync(UserId!, dto); 
            return HandleResult(result);
        }

        //DELETE BaseUrl/api/PatientAppointment/{id}
        [HttpDelete("{id}")]

        public async Task<IActionResult> CancelAppointment(int id)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Value.Length > 20)?.Value;

            var Result = await _appointmentService.CancelAppointmentAsync(id, UserId!);
            return HandleResult(Result);
        }

        //GET BaseUrl/api/PatientAppointment
        [HttpGet]   
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetPatientAppointments()
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Value.Length > 20)?.Value;

            var result = await _appointmentService.GetPatientAppointmentsAsync(UserId!);
            return HandleResult(result);
        }




    }
}
