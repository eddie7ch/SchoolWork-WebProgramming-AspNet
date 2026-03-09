using VehicleRental.Data;
using VehicleRental.Models;

namespace VehicleRental.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _db;
    public VehicleRepository(AppDbContext db) => _db = db;

    public IEnumerable<Vehicle> GetAll() => _db.Vehicles.ToList();

    public Vehicle? GetById(int id) => _db.Vehicles.Find(id);

    public void Add(Vehicle vehicle)
    {
        _db.Vehicles.Add(vehicle);
        _db.SaveChanges();
    }

    public void Update(Vehicle vehicle)
    {
        _db.Vehicles.Update(vehicle);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var vehicle = GetById(id);
        if (vehicle is null) return;
        _db.Vehicles.Remove(vehicle);
        _db.SaveChanges();
    }
}
