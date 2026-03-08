using VehicleRental.Models;

namespace VehicleRental.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private static readonly List<Vehicle> _vehicles =
    [
        new() { Id = 1, Make = "Toyota",   Model = "Camry",    Year = 2022, LicensePlate = "ABC-1234", VehicleType = VehicleType.Sedan,       DailyRate = 55.00m,  IsAvailable = true  },
        new() { Id = 2, Make = "Ford",     Model = "Explorer", Year = 2021, LicensePlate = "DEF-5678", VehicleType = VehicleType.SUV,         DailyRate = 75.00m,  IsAvailable = true  },
        new() { Id = 3, Make = "Dodge",    Model = "Ram 1500", Year = 2023, LicensePlate = "GHI-9012", VehicleType = VehicleType.Truck,       DailyRate = 90.00m,  IsAvailable = false },
        new() { Id = 4, Make = "Chrysler", Model = "Pacifica", Year = 2022, LicensePlate = "JKL-3456", VehicleType = VehicleType.Van,         DailyRate = 80.00m,  IsAvailable = true  },
        new() { Id = 5, Make = "Ford",     Model = "Mustang",  Year = 2023, LicensePlate = "MNO-7890", VehicleType = VehicleType.Convertible, DailyRate = 110.00m, IsAvailable = true  },
        new() { Id = 6, Make = "Honda",    Model = "Civic",    Year = 2022, LicensePlate = "PQR-1122", VehicleType = VehicleType.Hatchback,   DailyRate = 45.00m,  IsAvailable = true  },
    ];

    private static int _nextId = 7;

    public IEnumerable<Vehicle> GetAll() => _vehicles.ToList();

    public Vehicle? GetById(int id) => _vehicles.FirstOrDefault(v => v.Id == id);

    public void Add(Vehicle vehicle)
    {
        vehicle.Id = _nextId++;
        _vehicles.Add(vehicle);
    }

    public void Update(Vehicle vehicle)
    {
        var existing = GetById(vehicle.Id);
        if (existing is null) return;

        existing.Make         = vehicle.Make;
        existing.Model        = vehicle.Model;
        existing.Year         = vehicle.Year;
        existing.LicensePlate = vehicle.LicensePlate;
        existing.VehicleType  = vehicle.VehicleType;
        existing.DailyRate    = vehicle.DailyRate;
        existing.IsAvailable  = vehicle.IsAvailable;
    }

    public void Delete(int id)
    {
        var vehicle = GetById(id);
        if (vehicle is not null) _vehicles.Remove(vehicle);
    }
}
