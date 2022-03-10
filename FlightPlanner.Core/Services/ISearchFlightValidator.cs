using FlightPlanner.Core.DTO;

namespace FlightPlanner.Core.Services
{
    public interface ISearchFlightValidator
    {
        bool IsValid(SearchFlightRequest request);
    }
}
