using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class FromAirportNameValidator : IValidator
    {
        public bool Validate(AddFlightRequest request)
        {
            return !string.IsNullOrEmpty(request?.From?.Airport);
        }
    }
}
