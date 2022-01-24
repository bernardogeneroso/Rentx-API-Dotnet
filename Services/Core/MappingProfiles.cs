using AutoMapper;
using Models;
using Services.Cars;
using Services.CarsDetails;
using Services.CarsImages;

namespace Services.Core;


public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Car, Car>();
        CreateMap<CarDetail, CarDetail>();
        CreateMap<CarDetail, CarDetailDto>();
        CreateMap<Car, CarDto>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.CarImages.OrderByDescending(x => x.IsMain)))
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.CarDetails));
        CreateMap<CarImage, CarImageDto>()
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.ImageName));
    }
}

