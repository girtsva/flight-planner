using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class FromAirportValidator : IValidator
    {
        public bool Validate(AddFlightRequest request)
        {
            return request?.From != null;
        }
    }
}
