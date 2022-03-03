using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;

namespace FlightPlanner.Storage
{
    public static class FlightStorage
    {
        private static readonly object _flightLock = new object();

        //public static Flight AddFlight(AddFlightDto dto, IFlightService flightService)
        //{
        //    lock (_flightLock)
        //    {
        //        var flight = new Flight
        //        {
        //            From = new Airport
        //            {
        //                AirportName = dto.From.Airport,
        //                City = dto.From.City,
        //                Country = dto.From.Country
        //            },
        //            To = new Airport
        //            {
        //                AirportName = dto.To.Airport,
        //                City = dto.To.City,
        //                Country = dto.To.Country
        //            },
        //            ArrivalTime = dto.ArrivalTime,
        //            DepartureTime = dto.DepartureTime,
        //            Carrier = dto.Carrier
        //        };

        //        flightService.Create(flight);

        //        return flight;
        //    }
        //}

        public static Flight GetFlight(int id, IFlightPlannerDbContext context)
        {
            lock (_flightLock)
            {
                return context.Flights
                    .Include(flight => flight.From)
                    .Include(flight => flight.To)
                    .SingleOrDefault(flight => flight.Id == id);
            }
        }

        public static void DeleteFlight(int id, IFlightPlannerDbContext context)
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

        public static List<Airport> FindAirports(string userInput, IFlightPlannerDbContext context)
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

        public static void ClearFlights(IFlightPlannerDbContext context)
        {
            context.Flights.RemoveRange(context.Flights);
            context.Airports.RemoveRange(context.Airports);
            context.SaveChanges();
        }

        //public static bool FlightExistsInStorage(AddFlightRequest request, IFlightPlannerDbContext context)
        //{
        //    lock (_flightLock)
        //    {
        //        return context.Flights.Any(flight => flight.Carrier.ToLower().Trim() == request.Carrier.ToLower().Trim() &&
        //                                      flight.From.AirportName.ToLower().Trim() == request.From.Airport.ToLower().Trim() &&
        //                                      flight.To.AirportName.ToLower().Trim() == request.To.Airport.ToLower().Trim() &&
        //                                      flight.DepartureTime == request.DepartureTime &&
        //                                      flight.ArrivalTime == request.ArrivalTime);
        //    }
        //}

        public static bool IsValidAddFlightRequest(AddFlightDto dto)
        {
            lock (_flightLock)
            {
                if (dto == null)
                    return false;

                if (string.IsNullOrEmpty(dto.Carrier) || string.IsNullOrEmpty(dto.DepartureTime) ||
                    string.IsNullOrEmpty(dto.ArrivalTime))
                    return false;

                if (dto.From == null || dto.To == null)
                    return false;

                if (string.IsNullOrEmpty(dto.From.Airport) || string.IsNullOrEmpty(dto.From.City) ||
                    string.IsNullOrEmpty(dto.From.Country))
                    return false;

                if (string.IsNullOrEmpty(dto.To.Airport) || string.IsNullOrEmpty(dto.To.City) ||
                    string.IsNullOrEmpty(dto.To.Country))
                    return false;

                if (dto.From.Country.ToLower().Trim() == dto.To.Country.ToLower().Trim() &&
                    dto.From.City.ToLower().Trim() == dto.To.City.ToLower().Trim() &&
                    dto.From.Airport.ToLower().Trim() == dto.To.Airport.ToLower().Trim())
                    return false;

                var arrivalTime = DateTime.Parse(dto.ArrivalTime);
                var departureTime = DateTime.Parse(dto.DepartureTime);

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

        public static PageResult SearchFlights(SearchFlightRequest request, IFlightPlannerDbContext context)
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
