using Microsoft.EntityFrameworkCore;
using VehicleRental.Data;
using VehicleRental.Models;

namespace VehicleRental.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _db;
    public ReservationRepository(AppDbContext db) => _db = db;

    public IEnumerable<Reservation> GetAll() =>
        _db.Reservations
           .Include(r => r.Customer)
           .Include(r => r.Vehicle)
           .ToList();

    public Reservation? GetById(int id) =>
        _db.Reservations
           .Include(r => r.Customer)
           .Include(r => r.Vehicle)
           .FirstOrDefault(r => r.Id == id);

    public void Add(Reservation reservation)
    {
        _db.Reservations.Add(reservation);
        _db.SaveChanges();
    }

    public void Update(Reservation reservation)
    {
        _db.Reservations.Update(reservation);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var reservation = _db.Reservations.Find(id);
        if (reservation is null) return;
        _db.Reservations.Remove(reservation);
        _db.SaveChanges();
    }
}
