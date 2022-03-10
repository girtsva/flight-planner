using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators.AddFlight
{
    public class ArrivalTimeValidator : IValidator
    {
        public bool IsValid(AddFlightDto dto)
        {
            return !string.IsNullOrEmpty(dto?.ArrivalTime);
        }
    }
}
