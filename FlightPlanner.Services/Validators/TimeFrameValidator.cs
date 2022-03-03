using System;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class TimeFrameValidator : IValidator
    {
        public bool IsValid(AddFlightDto dto)
        {
            var arrivalTime = DateTime.Parse(dto.ArrivalTime);
            var departureTime = DateTime.Parse(dto.DepartureTime);

            return arrivalTime > departureTime;
        }
    }
}
