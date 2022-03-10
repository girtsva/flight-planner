using System.Collections.Generic;
using System.Linq;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;

namespace FlightPlanner.Services
{
    public class AirportService : EntityService<Airport>, IAirportService
    {
        public AirportService(IFlightPlannerDbContext context) : base(context)
        {
        }

        public List<Airport> FindAirports(string userInput)
        {
            //lock (_flightLock)
            //{
                userInput = userInput.ToLower().Trim();

                var airports = Query().Where(airport =>
                        airport.AirportName.ToLower().Trim().Contains(userInput) ||
                        airport.Country.ToLower().Trim().Contains(userInput) ||
                        airport.City.ToLower().Trim().Contains(userInput))
                    .ToList();

                return airports;
            //}
        }
    }
}
