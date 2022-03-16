using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        Flight GetFlightWithAirports(int id);

        void DeleteFlightById(int id);

        bool FlightExistsInStorage(AddFlightDto dto);

        PageResult SearchFlights(SearchFlightRequest request);
    }
}
