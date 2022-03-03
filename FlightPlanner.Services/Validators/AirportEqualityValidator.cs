using System;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class AirportEqualityValidator : IValidator
    {
        public bool IsValid(AddFlightDto dto)
        {
            return !string.Equals(dto.From.Airport.Trim(), 
                                 dto.To.Airport.Trim(),
                                 StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
