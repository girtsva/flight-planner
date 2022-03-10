using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;
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
        private readonly IAirportService _airportService;
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;
        private readonly IEnumerable<ISearchFlightValidator> _validators;
        private static readonly object _flightLock = new object();

        public CustomerAPIController(IAirportService airportService, IFlightService flightService, IMapper mapper, IEnumerable<ISearchFlightValidator> validators)
        {
            _airportService = airportService;
            _flightService = flightService;
            _mapper = mapper;
            _validators = validators;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            var airports = _airportService.FindAirports(search);
            var airportsDto = new List<AddAirportDto>();

            // mapping to required request format
            foreach (var airport in airports)
            {
                airportsDto.Add(_mapper.Map<AddAirportDto>(airport));
            }
            
            return Ok(airportsDto);
        }

        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights(SearchFlightRequest request)
        {
            lock (_flightLock)
            {
                if (!_validators.All(validator => validator.IsValid(request)))
                {
                    return BadRequest();
                }

                return Ok(_flightService.SearchFlights(request));
            }
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlightById(int id)
        {
            var flight = _flightService.GetFlightWithAirports(id);

            return flight == null ? NotFound() : Ok(_mapper.Map<AddFlightDto>(flight));
        }
    }
}
