using AutoMapper;
using Models;
using Services.Cars;
using Services.CarsAppointments;
using Services.CarsDetails;
using Services.CarsImages;

namespace Services.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        string currentOrigin = null;

        CreateMap<Car, Car>();
        CreateMap<CarDetailDto, CarDetail>();
        CreateMap<CarDetail, CarDetailDto>();
        CreateMap<Car, CarDto>()
            .ForMember(dest => dest.Images, opt =>
                    opt.MapFrom(src => src.CarImages.
                        Select(x => new CarImageDto
                        {
                            ImageName = x.ImageName,
                            Url = $"{currentOrigin}/{x.ImageName}",
                            IsMain = x.IsMain
                        })));
        CreateMap<CarImage, CarImageDto>();
        CreateMap<CarAppointmentDto, CarAppointment>();
    }
}

