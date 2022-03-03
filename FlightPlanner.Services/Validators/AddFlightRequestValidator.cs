using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class AddFlightRequestValidator : IValidator
    {
        public bool IsValid(AddFlightDto dto)
        {
            return dto != null;
        }
    }
}
