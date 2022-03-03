using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class AddFlightRequestValidator : IValidator
    {
        public bool Validate(AddFlightRequest request)
        {
            return request != null;
        }
    }
}
