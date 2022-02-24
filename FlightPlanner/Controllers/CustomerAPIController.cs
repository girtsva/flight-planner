using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [EnableCors]
    [ApiController]
    public class CustomerAPIController : ControllerBase
    {
        //private readonly FlightPlannerDbContext _context;
        private static readonly object _flightLock = new object();

        //public CustomerAPIController(FlightPlannerDbContext context)
        //{
        //    _context = context;
        //}

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            var airports = FlightStorage.FindAirports(search, new FlightPlannerDbContext());
            
            return Ok(airports);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightRequest request)
        {
            lock (_flightLock)
            {
                if (!FlightStorage.IsValidSearchRequest(request))
                {
                    return BadRequest();
                }

                return Ok(FlightStorage.SearchFlights(request, new FlightPlannerDbContext()));
            }
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult SearchFlights(int id)
        {
            var flight = FlightStorage.GetFlight(id, new FlightPlannerDbContext());

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}
