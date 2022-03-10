using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators.AddFlight
{
    public class AddFlightRequestValidator : IValidator
    {
        public bool IsValid(AddFlightDto dto)
        {
            return dto != null;
        }
    }
}
