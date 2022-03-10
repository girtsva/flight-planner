using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators.SearchFlight
{
    public class ToValidator : ISearchFlightValidator
    {
        public bool IsValid(SearchFlightRequest request)
        {
            return !string.IsNullOrEmpty(request?.To);
        }
    }
}
