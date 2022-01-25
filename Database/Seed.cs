using Microsoft.AspNetCore.Identity;
using Models;

namespace Database;

public class Seed
{
    public static async Task SeedData(UserManager<AppUser> userManager, DataContext context)
    {
        var users = new List<AppUser>();

        if (!userManager.Users.Any())
        {
            users = new List<AppUser>
            {
                new AppUser
                {
                  DisplayName = "Bob",
                  UserName = "bob",
                  Email = "bob@test.com",
                  Role = Role.User
                },
                new AppUser
                {
                  DisplayName = "Jim",
                  UserName = "jim",
                  Email = "jim@test.com",
                  Role = Role.User
                },
                new AppUser
                {
                  DisplayName = "Admin",
                  UserName = "admin",
                  Email = "admin@test.com",
                  Role = Role.Admin
                }
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

        }

        if (!context.Cars.Any())
        {
            var cars = new List<Car>
            {
                new Car
                {
                  Plate = "ABC123",
                  Brand = "Toyota",
                  Model = "Corolla",
                  Color = "White",
                  Year = "2020",
                  Fuel = "Diesel",
                  Transmission = "Automatic",
                  Doors = "5",
                  Seats = "5",
                  PricePerDay = 100,
                  CarDetails = new CarDetail
                  {
                    maxSpeed = 200,
                    topSpeed = 220,
                    acceleration = 2.5f,
                    weight = 1500,
                    hp = 150
                  }
                },
                new Car
                {
                    Plate = "DEF456",
                    Brand = "BMW",
                    Model = "M4",
                    Color = "White",
                    Year = "2021",
                    Fuel = "Gasoline",
                    Transmission = "Automatic",
                    Doors = "5",
                    Seats = "5",
                    PricePerDay = 999,
                    CarDetails = new CarDetail
                    {
                        maxSpeed = 300,
                        topSpeed = 320,
                        acceleration = 3.5f,
                        weight = 2000,
                        hp = 200
                    }
                },
                new Car
                {
                    Plate = "GHI789",
                    Brand = "Mercedes",
                    Model = "C63",
                    Color = "Black",
                    Year = "2022",
                    Fuel = "Diesel",
                    Transmission = "Automatic",
                    Doors = "5",
                    Seats = "5",
                    PricePerDay = 1200,
                    CarDetails = new CarDetail
                    {
                        maxSpeed = 400,
                        topSpeed = 420,
                        acceleration = 4.5f,
                        weight = 2500,
                        hp = 250
                    }
                },
                new Car
                {
                    Plate = "JKL012",
                    Brand = "Audi",
                    Model = "A6",
                    Color = "Red",
                    Year = "2023",
                    Fuel = "Gasoline",
                    Transmission = "Automatic",
                    Doors = "5",
                    Seats = "5",
                    PricePerDay = 1500,
                    CarDetails = new CarDetail
                    {
                        maxSpeed = 500,
                        topSpeed = 520,
                        acceleration = 5.5f,
                        weight = 3000,
                        hp = 300
                    }
                },
                new Car
                {
                    Plate = "MNO345",
                    Brand = "Ford",
                    Model = "Fiesta",
                    Color = "Blue",
                    Year = "2024",
                    Fuel = "Diesel",
                    Transmission = "Automatic",
                    Doors = "5",
                    Seats = "5",
                    PricePerDay = 1800,
                    CarDetails = new CarDetail
                    {
                        maxSpeed = 600,
                        topSpeed = 620,
                        acceleration = 6.5f,
                        weight = 3500,
                        hp = 350
                    }
                },
            };

            await context.Cars.AddRangeAsync(cars);

            if (!context.CarsAppointments.Any())
            {
                var carsAppointments = new List<CarAppointment>
                {
                    new CarAppointment
                    {
                        Id = Guid.NewGuid(),
                        Car = cars[0],
                        User = users[2],
                        StartDate = new DateTime(2022, 1, 1),
                        EndDate = new DateTime(2022, 1, 1).AddDays(2),
                        RentalPrice = cars[0].PricePerDay * 2
                    },
                    new CarAppointment
                    {
                        Id = Guid.NewGuid(),
                        Car = cars[1],
                        User = users[2],
                        StartDate = new DateTime(2022, 1, 1),
                        EndDate = new DateTime(2022, 1, 1).AddDays(2),
                        RentalPrice = cars[1].PricePerDay * 2
                    },
                    new CarAppointment
                    {
                        Id = Guid.NewGuid(),
                        Car = cars[2],
                        User = users[2],
                        StartDate = new DateTime(2022, 1, 5),
                        EndDate = new DateTime(2022, 1, 5).AddDays(3),
                        RentalPrice = cars[2].PricePerDay * 3
                    },
                    new CarAppointment
                    {
                        Id = Guid.NewGuid(),
                        Car = cars[3],
                        User = users[2],
                        StartDate = new DateTime(2022, 1, 8),
                        EndDate = new DateTime(2022, 1, 8).AddDays(5),
                        RentalPrice = cars[3].PricePerDay * 5
                    },
                    new CarAppointment
                    {
                        Id = Guid.NewGuid(),
                        Car = cars[4],
                        User = users[2],
                        StartDate = new DateTime(2022, 1, 30),
                        EndDate = new DateTime(2022, 1, 30).AddDays(2),
                        RentalPrice = cars[4].PricePerDay * 2
                    },
                    new CarAppointment
                    {
                        Id = Guid.NewGuid(),
                        Car = cars[4],
                        User = users[2],
                        StartDate = new DateTime(2022, 2, 1),
                        EndDate = new DateTime(2022, 2, 1).AddDays(4),
                        RentalPrice = cars[4].PricePerDay * 4
                    },
                    new CarAppointment
                    {
                        Id = Guid.NewGuid(),
                        Car = cars[3],
                        User = users[2],
                        StartDate = new DateTime(2022, 2, 5),
                        EndDate = new DateTime(2022, 2, 5).AddDays(2),
                        RentalPrice = cars[3].PricePerDay * 2
                    },
                };

                await context.CarsAppointments.AddRangeAsync(carsAppointments);
            }

            await context.SaveChangesAsync();
        }
    }
}
