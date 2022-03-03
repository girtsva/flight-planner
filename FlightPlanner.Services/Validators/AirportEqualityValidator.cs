using System;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class AirportEqualityValidator : IValidator
    {
        public bool Validate(AddFlightRequest request)
        {
            return !string.Equals(request.From.Airport.Trim(), 
                                 request.To.Airport.Trim(),
                                 StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
