using System;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class TimeFrameValidator : IValidator
    {
        public bool Validate(AddFlightRequest request)
        {
            var arrivalTime = DateTime.Parse(request.ArrivalTime);
            var departureTime = DateTime.Parse(request.DepartureTime);

            return arrivalTime > departureTime;
        }
    }
}
