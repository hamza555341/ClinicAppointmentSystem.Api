using ClinicAppointment.Service.Abstraction;
using ClinicAppointment.Shared.DTOs.AppointmentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ClinicAppointment.Presentation.Controllers.Admin
{
    [Authorize(Roles = "Admin")]

    public class AdminAppointmentController:ApiBaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AdminAppointmentController(IAppointmentService appointmentService)
        {
           _appointmentService = appointmentService;
        }

        // GET: BaseUrl/api/AdminAppointment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAllAppointments()
        {
            var result = await _appointmentService.GetAllAppointmentAsync();
            return HandleResult(result);
        }
        // PUT: BaseUrl/api/AdminAppointment/status

        [HttpPut("status")]
        public async Task<IActionResult> ChangeStatus(ChangeAppointmentStatusDTO dto)
        {
            var result = await _appointmentService.ChangeAppointmentStatusAsync(dto);
            return HandleResult(result);

        }

    }
}
