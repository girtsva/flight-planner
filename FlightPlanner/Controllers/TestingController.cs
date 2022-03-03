using FlightPlanner.Data;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [EnableCors]
    [ApiController]
    public class TestingController : ControllerBase
    {
        private readonly IFlightPlannerDbContext _context;

        public TestingController(IFlightPlannerDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        [Route("clear")]
        public IActionResult Clear()
        {
            FlightStorage.ClearFlights(_context);

            return Ok();
        }
    }
}
