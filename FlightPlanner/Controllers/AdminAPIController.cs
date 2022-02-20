using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [EnableCors]
    [ApiController]
    [Authorize]
    public class AdminAPIController : ControllerBase
    {
        private static readonly object _flightLock = new object();

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlights(int id)
        {
            var flight = FlightStorage.GetFlight(id);

            if (flight == null)
            {
                return NotFound();
            }
                
            return Ok(flight);
        }

        [HttpPut, Authorize]
        [Route("flights")]
        public IActionResult PutFlights(AddFlightRequest request)
        {
            lock (_flightLock)
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

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlights(int id)
        {
            lock (_flightLock)
            {
                FlightStorage.DeleteFlight(id);

                return Ok();
            }
        }
    }
}
