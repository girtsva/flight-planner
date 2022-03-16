using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators.SearchFlight
{
    public class SearchFlightRequestValidator : ISearchFlightValidator
    {
        public bool IsValid(SearchFlightRequest request)
        {
            return request != null;
        }
    }
}
