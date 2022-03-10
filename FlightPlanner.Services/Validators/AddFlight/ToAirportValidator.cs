using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators.AddFlight
{
    public class ToAirportValidator : IValidator
    {
        public bool IsValid(AddFlightDto dto)
        {
            return dto?.To != null;
        }
    }
}
