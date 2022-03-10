using System;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Services;

namespace FlightPlanner.Services.Validators.SearchFlight
{
    public class FromToEqualityValidator : ISearchFlightValidator
    {
        public bool IsValid(SearchFlightRequest request)
        {
            return !string.Equals(request.From.Trim(), request.To.Trim(), StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
