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
        CreateMap<Car, CarDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images.Select(x => new CarImageDto
            {
                ImageName = x.ImageName,
                Url = $"{currentOrigin}/{x.ImageName}",
                IsMain = x.IsMain
            }).FirstOrDefault(x => x.IsMain)));
        CreateMap<Car, CarScheduledDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images.Select(x => new CarImageDto
            {
                ImageName = x.ImageName,
                Url = $"{currentOrigin}/{x.ImageName}",
                IsMain = x.IsMain
            }).FirstOrDefault(x => x.IsMain)))
            .ForMember(dest => dest.Appointment, opt =>
                    opt.MapFrom(src => src.Appointments.FirstOrDefault(x => x.Car.Plate == src.Plate)));
        CreateMap<Car, FavoriteCarDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images.Select(x => new CarImageDto
                {
                    ImageName = x.ImageName,
                    Url = $"{currentOrigin}/{x.ImageName}",
                    IsMain = x.IsMain
                }).FirstOrDefault(x => x.IsMain)));

        CreateMap<CarDetail, CarDetail>();
        CreateMap<CarDetail, CarDetailDto>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Car.Images.Select(x => new CarImageDto
            {
                ImageName = x.ImageName,
                Url = $"{currentOrigin}/{x.ImageName}",
                IsMain = x.IsMain
            }).OrderByDescending(x => x.IsMain)));

        CreateMap<CarImage, CarImageDto>();

        CreateMap<CarAppointment, CarAppointmentDto>();
    }
}

