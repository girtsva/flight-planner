using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class FromAirportValidator : IValidator
    {
        public bool IsValid(AddFlightDto dto)
        {
            return dto?.From != null;
        }
    }
}
