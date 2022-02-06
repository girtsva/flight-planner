using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminAPIController : ControllerBase
    {
        [HttpGet]
        [Route("Flights/{id}")]
        public IActionResult GetFlights(int id)
        {
            return NotFound();
        }

        [HttpPut, Authorize]
        [Route("flights")]
        public IActionResult PutFlights(AddFlightRequest request)
        {
            if (!FlightStorage.IsValid(request))
            {
                return BadRequest();
            }
            
            if (FlightStorage.Exists(request))
            {
                return Conflict();
            }
            
            return Created("", FlightStorage.AddFlight(request));
        }
    }
}
