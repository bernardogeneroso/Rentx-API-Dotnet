using AutoMapper;
using Models;
using Services.Cars.DTOs;
using Services.CarsAppointments.DTOs;
using Services.CarsDetails.DTOs;
using Services.CarsImages.DTOs;

namespace Services.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        string currentOrigin = null;

        CreateMap<Car, Car>();
        CreateMap<Car, CarDtoQuery>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images.Select(x => new CarImageDtoQuery
            {
                ImageName = x.ImageName,
                Url = $"{currentOrigin}/images/{x.ImageName}",
                IsMain = x.IsMain
            }).FirstOrDefault(x => x.IsMain)));
        CreateMap<Car, CarScheduledDtoQuery>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images.Select(x => new CarImageDtoQuery
            {
                ImageName = x.ImageName,
                Url = $"{currentOrigin}/images/{x.ImageName}",
                IsMain = x.IsMain
            }).FirstOrDefault(x => x.IsMain)))
            .ForMember(dest => dest.Appointment, opt =>
                    opt.MapFrom(src => src.Appointments.FirstOrDefault(x => x.Car.Plate == src.Plate)));
        CreateMap<CarDtoRequest, Car>();
        CreateMap<CarDtoQuery, FavoriteCarDtoQuery>();

        CreateMap<CarDetailDtoRequest, CarDetail>();
        CreateMap<CarDetail, CarDetailDtoQuery>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Car.Images.Select(x => new CarImageDtoQuery
            {
                ImageName = x.ImageName,
                Url = $"{currentOrigin}/images/{x.ImageName}",
                IsMain = x.IsMain
            }).OrderByDescending(x => x.IsMain)));

        CreateMap<CarImage, CarImageDtoQuery>();

        CreateMap<CarAppointment, CarAppointmentDtoQuery>();
    }
}

