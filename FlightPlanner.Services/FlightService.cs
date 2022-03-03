using System.Linq;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;

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
            //lock (_flightLock)
            //{
                return Query().Any(flight => flight.Carrier.ToLower().Trim() == dto.Carrier.ToLower().Trim() &&
                                                     flight.From.AirportName.ToLower().Trim() == dto.From.Airport.ToLower().Trim() &&
                                                     flight.To.AirportName.ToLower().Trim() == dto.To.Airport.ToLower().Trim() &&
                                                     flight.DepartureTime == dto.DepartureTime &&
                                                     flight.ArrivalTime == dto.ArrivalTime);
            //}
        }
    }
}
