using FlightPlanner.Core.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [EnableCors]
    [ApiController]
    public class TestingController : ControllerBase
    {
        private readonly IDbClearService _service;
        private static readonly object _flightLock = new object();

        public TestingController(IDbClearService service)
        {
            _service = service;
        }
        
        [HttpPost]
        [Route("clear")]
        public IActionResult Clear()
        {
            lock (_flightLock)
            {
                _service.DeleteAll();

                return Ok();
            }
        }
    }
}
