using AutoMapper;
using FlightPlanner.Core.DTO;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Services.Mappers
{
    public class AutoMapperConfig
    {
        public static IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Flight, AddFlightDto>();
                cfg.CreateMap<AddFlightDto, Flight>();
                cfg.CreateMap<Airport, AddAirportDto>()
                    .ForMember(dest => dest.Airport, opt => opt.MapFrom(airport => airport.AirportName));
                cfg.CreateMap<AddAirportDto, Airport>()
                    .ForMember(dest => dest.AirportName, opt => opt.MapFrom(airport => airport.Airport))
                    .ForMember(dest => dest.Id, opt => opt.Ignore());
            });
            // only during development, validate your mappings; remove it before release
            configuration.AssertConfigurationIsValid();
            // use DI (http://docs.automapper.org/en/latest/Dependency-injection.html) or create the mapper yourself
            var mapper = configuration.CreateMapper();
            return mapper;
        }
    }
}
