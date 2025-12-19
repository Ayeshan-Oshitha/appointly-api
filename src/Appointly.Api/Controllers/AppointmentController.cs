using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointly.Api.Controllers
{
    [Route("appointment")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAppointments()
        {
            return Ok("This is a placeholder for appointment retrieval.");
        }
    }
}
