using AutoMapper;
using Models;
using Services.Cars;

namespace Services.Core;


public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Car, Car>();
        CreateMap<Car, CarDto>();
    }
}

