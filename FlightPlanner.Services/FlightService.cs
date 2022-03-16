using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FlightPlanner.Services
{
    public class FlightService : EntityService<Flight>, IFlightService
    {
        public FlightService(IFlightPlannerDbContext context) : base(context)
        {
        }

        public Flight GetFlightWithAirports(int id)
        {
            return Query()
                .Include(flight => flight.From)
                .Include(flight => flight.To)
                .SingleOrDefault(flight => flight.Id == id);
        }

        public void DeleteFlightById(int id)
        {
            var flight = GetFlightWithAirports(id);

            if(flight != null)
                Delete(flight);
        }

        public bool FlightExistsInStorage(AddFlightDto dto)
        {
            return Query().Any(flight => flight.Carrier.ToLower().Trim() == dto.Carrier.ToLower().Trim() &&
                                         flight.From.AirportName.ToLower().Trim() == dto.From.Airport.ToLower().Trim() &&
                                         flight.To.AirportName.ToLower().Trim() == dto.To.Airport.ToLower().Trim() &&
                                         flight.DepartureTime == dto.DepartureTime &&
                                         flight.ArrivalTime == dto.ArrivalTime);
        }

        public PageResult SearchFlights(SearchFlightRequest request)
        {
            var foundFlights = Query()
                    .Include(flight => flight.From)
                    .Include(flight => flight.To)
                    .Where(flight =>
                        flight.From.AirportName.ToLower().Trim() == request.From.ToLower().Trim() &&
                        flight.To.AirportName.ToLower().Trim() == request.To.ToLower().Trim() &&
                        flight.DepartureTime.Substring(0, 10) == request.DepartureDate.Substring(0, 10)).ToList();

            return new PageResult(foundFlights);
        }
    }
}
