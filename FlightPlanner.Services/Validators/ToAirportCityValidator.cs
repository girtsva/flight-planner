using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators
{
    public class ToAirportCityValidator : IValidator
    {
        public bool Validate(AddFlightRequest request)
        {
            return !string.IsNullOrEmpty(request?.To?.City);
        }
    }
}
