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

        public bool FlightExistsInStorage(AddFlightRequest request)
        {
            //lock (_flightLock)
            //{
                return Query().Any(flight => flight.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() &&
                                                     flight.From.AirportName.ToLower().Trim() == request.From.Airport.ToLower().Trim() &&
                                                     flight.To.AirportName.ToLower().Trim() == request.To.Airport.ToLower().Trim() &&
                                                     flight.DepartureTime == request.DepartureTime &&
                                                     flight.ArrivalTime == request.ArrivalTime);
            //}
        }
    }
}
