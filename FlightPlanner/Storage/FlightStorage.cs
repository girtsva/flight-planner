using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner.Storage
{
    public static class FlightStorage
    {
        private static readonly object _flightLock = new object();

        public static Flight AddFlight(AddFlightRequest request, FlightPlannerDbContext context)
        {
            lock (_flightLock)
            {
                var flight = new Flight
                {
                    From = request.From,
                    To = request.To,
                    ArrivalTime = request.ArrivalTime,
                    DepartureTime = request.DepartureTime,
                    Carrier = request.Carrier
                };

                context.Flights.Add(flight);
                context.SaveChanges();

                return flight;
            }
        }

        public static Flight GetFlight(int id, FlightPlannerDbContext context)
        {
            lock (_flightLock)
            {
                return context.Flights
                    .Include(flight => flight.From)
                    .Include(flight => flight.To)
                    .SingleOrDefault(flight => flight.Id == id);
            }
        }

        public static void DeleteFlight(int id, FlightPlannerDbContext context)
        {
            lock (_flightLock)
            {
                var flight = GetFlight(id, context);

                if (flight != null)
                {
                    context.Flights.Remove(flight);
                    context.SaveChanges();
                }
            }
        }

        public static List<Airport> FindAirports(string userInput, FlightPlannerDbContext context)
        {
            lock (_flightLock)
            {
                userInput = userInput.ToLower().Trim();

                var fromAirport = context.Flights.Where(flight =>
                        flight.From.AirportName.ToLower().Trim().Contains(userInput) ||
                        flight.From.Country.ToLower().Trim().Contains(userInput) ||
                        flight.From.City.ToLower().Trim().Contains(userInput))
                    .Select(airport => airport.From).ToList();

                var toAirport = context.Flights.Where(flight =>
                        flight.To.AirportName.ToLower().Trim().Contains(userInput) ||
                        flight.To.Country.ToLower().Trim().Contains(userInput) ||
                        flight.To.City.ToLower().Trim().Contains(userInput))
                    .Select(airport => airport.To).ToList();

                return fromAirport.Concat(toAirport).ToList();
            }
        }

        public static void ClearFlights(FlightPlannerDbContext context)
        {
            context.RemoveRange(context.Flights);
            context.RemoveRange(context.Airports);
            context.SaveChanges();
        }

        public static bool FlightExistsInStorage(AddFlightRequest request, FlightPlannerDbContext context)
        {
            lock (_flightLock)
            {
                return context.Flights.Any(flight => flight.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() &&
                                              flight.From.AirportName.ToLower().Trim() == request.From.AirportName.ToLower().Trim() &&
                                              flight.To.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim() &&
                                              flight.DepartureTime == request.DepartureTime &&
                                              flight.ArrivalTime == request.ArrivalTime);
            }
        }

        public static bool IsValidAddFlightRequest(AddFlightRequest request)
        {
            lock (_flightLock)
            {
                if (request == null)
                    return false;

                if (string.IsNullOrEmpty(request.Carrier) || string.IsNullOrEmpty(request.DepartureTime) ||
                    string.IsNullOrEmpty(request.ArrivalTime))
                    return false;

                if (request.From == null || request.To == null)
                    return false;

                if (string.IsNullOrEmpty(request.From.AirportName) || string.IsNullOrEmpty(request.From.City) ||
                    string.IsNullOrEmpty(request.From.Country))
                    return false;

                if (string.IsNullOrEmpty(request.To.AirportName) || string.IsNullOrEmpty(request.To.City) ||
                    string.IsNullOrEmpty(request.To.Country))
                    return false;

                if (request.From.Country.ToLower().Trim() == request.To.Country.ToLower().Trim() &&
                    request.From.City.ToLower().Trim() == request.To.City.ToLower().Trim() &&
                    request.From.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim())
                    return false;

                var arrivalTime = DateTime.Parse(request.ArrivalTime);
                var departureTime = DateTime.Parse(request.DepartureTime);

                if (arrivalTime <= departureTime)
                    return false;

                return true;
            }
        }

        public static bool IsValidSearchFlightRequest(SearchFlightRequest request)
        {
            lock (_flightLock)
            {
                if (request == null)
                    return false;

                if (string.IsNullOrEmpty(request.From) || string.IsNullOrEmpty(request.To) ||
                    string.IsNullOrEmpty(request.DepartureDate))
                    return false;

                if (request.From.ToLower().Trim() == request.To.ToLower().Trim())
                    return false;

                return true;
            }
        }

        public static PageResult SearchFlights(SearchFlightRequest request, FlightPlannerDbContext context)
        {
            lock (_flightLock)
            {
                var foundFlights = context.Flights
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
}
