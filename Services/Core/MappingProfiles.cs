using AutoMapper;
using Models;
using Services.Cars;
using Services.CarsImage;

namespace Services.Core;


public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Car, Car>();
        CreateMap<Car, CarDto>();
        CreateMap<CarImage, CarImageDto>();
    }
}

