using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [EnableCors]
    [ApiController]
    [Authorize]
    public class AdminAPIController : ControllerBase
    {
        private readonly FlightPlannerDbContext _context;
        private static readonly object _flightLock = new object();

        public AdminAPIController(FlightPlannerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = FlightStorage.GetFlight(id, _context);

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
                if (!FlightStorage.IsValidAddFlightRequest(request))
                {
                    return BadRequest();
                }

                if (FlightStorage.FlightExistsInStorage(request, _context))
                {
                    return Conflict();
                }

                return Created("", FlightStorage.AddFlight(request, _context));
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (_flightLock)
            {
                FlightStorage.DeleteFlight(id, _context);

                return Ok();
            }
        }
    }
}
