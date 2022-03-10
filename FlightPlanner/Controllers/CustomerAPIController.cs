using System.Collections.Generic;
using AutoMapper;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
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
        private readonly IMapper _mapper;
        private readonly IFlightPlannerDbContext _context;
        private static readonly object _flightLock = new object();

        public CustomerAPIController(IAirportService airportService, IMapper mapper, IFlightPlannerDbContext context)
        {
            _airportService = airportService;
            _context = context;
            _mapper = mapper;
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
                if (!FlightStorage.IsValidSearchFlightRequest(request))
                {
                    return BadRequest();
                }

                return Ok(FlightStorage.SearchFlights(request, _context));
            }
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult FindFlightById(int id)
        {
            var flight = FlightStorage.GetFlight(id, _context);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}
