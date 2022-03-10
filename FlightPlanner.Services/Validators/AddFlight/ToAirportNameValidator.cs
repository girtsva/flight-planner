using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators.AddFlight
{
    public class ToAirportNameValidator : IValidator
    {
        public bool IsValid(AddFlightDto dto)
        {
            return !string.IsNullOrEmpty(dto?.To?.Airport);
        }
    }
}
