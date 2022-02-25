using System.Linq;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult GetFlights(int id)
        {
            var flight = _context.Flights
                .Include(flight => flight.From)
                .Include(flight => flight.To)
                .SingleOrDefault(flight => flight.Id == id);
                //FlightStorage.GetFlight(id);

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

                if (FlightStorage.Exists(request, _context))
                {
                    return Conflict();
                }

                var flight = FlightStorage.ConvertToFlight(request);
                _context.Flights.Add(flight);
                _context.SaveChanges();

                return Created("", flight);
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlights(int id)
        {
            lock (_flightLock)
            {
                var flight = _context.Flights
                    .Include(flight => flight.From)
                    .Include(flight => flight.To)
                    .SingleOrDefault(flight => flight.Id == id);

                if (flight != null)
                {
                    _context.Flights.Remove(flight);
                    _context.SaveChanges();
                }

                return Ok();
            }
        }

        private bool Exists(AddFlightRequest request)  // nosaukumu pamainit -> FlightExistsInStorage
        {
            return _context.Flights.Any(flight => flight.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() &&
                                   flight.From.AirportName.ToLower().Trim() == request.From.AirportName.ToLower().Trim() &&
                                   flight.To.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim() &&
                                   flight.DepartureTime == request.DepartureTime &&
                                   flight.ArrivalTime == request.ArrivalTime);
        }
    }
}
