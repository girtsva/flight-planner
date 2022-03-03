namespace FlightPlanner.Core.Services
{
    public interface IDbClearService : IDbService
    {
        void DeleteAll();
    }
}
