using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("admin-api")]
    [EnableCors]
    [ApiController]
    [Authorize]
    public class AdminAPIController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IEnumerable<IValidator> _validators;
        private static readonly object _flightLock = new object();

        public AdminAPIController(IFlightService flightService, IEnumerable<IValidator> validators)
        {
            _flightService = flightService;
            _validators = validators;
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlight(int id)
        {
            var flight = _flightService.GetFlightWithAirports(id);

            return flight == null ? NotFound() : Ok(flight);
        }

        [HttpPut, Authorize]
        [Route("flights")]
        public IActionResult PutFlights(AddFlightRequest request)
        {
            lock (_flightLock)
            {
                if (!_validators.All(validator => validator.Validate(request)))
                {
                    return BadRequest();
                }

                if (_flightService.FlightExistsInStorage(request))
                {
                    return Conflict();
                }

                return Created("", FlightStorage.AddFlight(request, _flightService));
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            lock (_flightLock)
            {
                _flightService.DeleteFlightById(id);
                //FlightStorage.DeleteFlight(id, _context);

                return Ok();
            }
        }
    }
}
