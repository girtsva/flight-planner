using FlightPlanner.Core.DTO;
using FlightPlanner.Models;

namespace FlightPlanner.Core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        Flight GetFlightWithAirports(int id);

        void DeleteFlightById(int id);

        bool FlightExistsInStorage(AddFlightRequest request);
    }
}
