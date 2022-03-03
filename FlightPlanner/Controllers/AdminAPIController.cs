using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using FlightPlanner.Models;
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
        private readonly IMapper _mapper;
        private static readonly object _flightLock = new object();

        public AdminAPIController(IFlightService flightService, IEnumerable<IValidator> validators, IMapper mapper)
        {
            _flightService = flightService;
            _validators = validators;
            _mapper = mapper;
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
        public IActionResult PutFlights(AddFlightDto dto)
        {
            lock (_flightLock)
            {
                if (!_validators.All(validator => validator.IsValid(dto)))
                {
                    return BadRequest();
                }

                if (_flightService.FlightExistsInStorage(dto))
                {
                    return Conflict();
                }

                var flight = _mapper.Map<Flight>(dto);

                _flightService.Create(flight);

                return Created("", _mapper.Map<AddFlightDto>(flight));
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
