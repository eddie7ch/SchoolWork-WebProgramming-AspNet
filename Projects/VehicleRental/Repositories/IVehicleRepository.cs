using VehicleRental.Models;

namespace VehicleRental.Repositories;

public interface IVehicleRepository
{
    IEnumerable<Vehicle> GetAll();
    Vehicle? GetById(int id);
    void Add(Vehicle vehicle);
    void Update(Vehicle vehicle);
    void Delete(int id);
}
