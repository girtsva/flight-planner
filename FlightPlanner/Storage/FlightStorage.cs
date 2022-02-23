using FlightPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightPlanner.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id;
        private static readonly object _flightLock = new object();

        public static Flight AddFlight(AddFlightRequest request)
        {
            lock (_flightLock)
            {
                var flight = new Flight
                {
                    From = request.From,
                    To = request.To,
                    ArrivalTime = request.ArrivalTime,
                    DepartureTime = request.DepartureTime,
                    Carrier = request.Carrier,
                    Id = ++_id
                };

                _flights.Add(flight);

                return flight;
            }
        }

        public static Flight GetFlight(int id)
        {
            lock (_flightLock)
            {
                return _flights.SingleOrDefault(flight => flight.Id == id);
            }
        }

        public static void DeleteFlight(int id)
        {
            lock (_flightLock)
            {
                var flight = GetFlight(id);

                if (flight != null)
                    _flights.Remove(flight);
            }
        }

        public static List<Airport> FindAirports(string userInput)
        {
            lock (_flightLock)
            {
                userInput = userInput.ToLower().Trim();

                var fromAirport = _flights.Where(flight =>
                        flight.From.AirportName.ToLower().Trim().Contains(userInput) ||
                        flight.From.Country.ToLower().Trim().Contains(userInput) ||
                        flight.From.City.ToLower().Trim().Contains(userInput))
                    .Select(airport => airport.From).ToList();

                var toAirport = _flights.Where(flight =>
                        flight.To.AirportName.ToLower().Trim().Contains(userInput) ||
                        flight.To.Country.ToLower().Trim().Contains(userInput) ||
                        flight.To.City.ToLower().Trim().Contains(userInput))
                    .Select(airport => airport.To).ToList();

                return fromAirport.Concat(toAirport).ToList();
            }
        }

        public static void ClearFlights()
        {
            _flights.Clear();
            _id = 0;
        }

        public static bool Exists(AddFlightRequest request)
        {
            lock (_flightLock)
            {
                return _flights.Any(flight => flight.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() &&
                                              flight.From.AirportName.ToLower().Trim() == request.From.AirportName.ToLower().Trim() &&
                                              flight.To.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim() &&
                                              flight.DepartureTime == request.DepartureTime &&
                                              flight.ArrivalTime == request.ArrivalTime);
            }
        }

        public static bool IsValid(AddFlightRequest request)
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

        public static bool IsValidSearchRequest(SearchFlightRequest request)
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

        public static PageResult SearchFlights(SearchFlightRequest request)
        {
            lock (_flightLock)
            {
                var foundFlights = _flights.Where(flight =>
                    flight.From.AirportName.ToLower().Trim() == request.From.ToLower().Trim() &&
                    flight.To.AirportName.ToLower().Trim() == request.To.ToLower().Trim() &&
                    DateTime.Parse(flight.DepartureTime).Date == DateTime.Parse(request.DepartureDate)).ToList();

                return new PageResult(foundFlights);
            }
        }
    }
}
