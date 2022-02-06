using System;
using FlightPlanner.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;

namespace FlightPlanner.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id;

        public static Flight AddFlight(AddFlightRequest request)
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

        public static Flight GetFlight(int id)
        {
            return _flights.SingleOrDefault(flight => flight.Id == id);
        }

        public static void DeleteFlight(int id)
        {
            var flight = GetFlight(id);
            if (flight != null)
                _flights.Remove(flight);
        }

        public static void ClearFlights()
        {
            _flights.Clear();
            _id = 0;
        }

        public static bool Exists(AddFlightRequest request)
        {
            return _flights.Any(flight => flight.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() &&
                                          flight.From.AirportName.ToLower().Trim() == request.From.AirportName.ToLower().Trim() &&
                                          flight.To.AirportName.ToLower().Trim() == request.To.AirportName.ToLower().Trim() &&
                                          flight.DepartureTime == request.DepartureTime &&
                                          flight.ArrivalTime == request.ArrivalTime);
        }

        public static bool IsValid(AddFlightRequest request)
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
}
