using Microsoft.EntityFrameworkCore;
using VehicleRental.Data;
using VehicleRental.Models;

namespace VehicleRental.Repositories;

public class BillRepository : IBillRepository
{
    private readonly AppDbContext _db;
    public BillRepository(AppDbContext db) => _db = db;

    public IEnumerable<Bill> GetAll() =>
        _db.Bills
           .Include(b => b.Reservation)
               .ThenInclude(r => r!.Customer)
           .Include(b => b.Reservation)
               .ThenInclude(r => r!.Vehicle)
           .ToList();

    public Bill? GetById(int id) =>
        _db.Bills
           .Include(b => b.Reservation)
               .ThenInclude(r => r!.Customer)
           .Include(b => b.Reservation)
               .ThenInclude(r => r!.Vehicle)
           .FirstOrDefault(b => b.Id == id);

    public Bill? GetByReservationId(int reservationId) =>
        _db.Bills
           .Include(b => b.Reservation)
           .FirstOrDefault(b => b.ReservationId == reservationId);

    public void Add(Bill bill)
    {
        _db.Bills.Add(bill);
        _db.SaveChanges();
    }

    public void Update(Bill bill)
    {
        _db.Bills.Update(bill);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var bill = _db.Bills.Find(id);
        if (bill is null) return;
        _db.Bills.Remove(bill);
        _db.SaveChanges();
    }
}
