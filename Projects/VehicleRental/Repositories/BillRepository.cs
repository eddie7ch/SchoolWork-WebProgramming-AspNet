using VehicleRental.Models;

namespace VehicleRental.Repositories;

public class BillRepository : IBillRepository
{
    private static readonly List<Bill> _bills =
    [
        new() { Id = 1, ReservationId = 1, BaseAmount = 270.00m, TaxRate = 10m, AdditionalCharges = 20.00m, IsPaid = true  },
        new() { Id = 2, ReservationId = 2, BaseAmount = 165.00m, TaxRate = 10m, AdditionalCharges = 0.00m,  IsPaid = false },
    ];

    private static int _nextId = 3;

    public IEnumerable<Bill> GetAll() => _bills.ToList();

    public Bill? GetById(int id) => _bills.FirstOrDefault(b => b.Id == id);

    public Bill? GetByReservationId(int reservationId) =>
        _bills.FirstOrDefault(b => b.ReservationId == reservationId);

    public void Add(Bill bill)
    {
        bill.Id = _nextId++;
        _bills.Add(bill);
    }

    public void Update(Bill bill)
    {
        var existing = GetById(bill.Id);
        if (existing is null) return;

        existing.ReservationId     = bill.ReservationId;
        existing.BaseAmount        = bill.BaseAmount;
        existing.TaxRate           = bill.TaxRate;
        existing.AdditionalCharges = bill.AdditionalCharges;
        existing.IsPaid            = bill.IsPaid;
    }

    public void Delete(int id)
    {
        var bill = GetById(id);
        if (bill is not null) _bills.Remove(bill);
    }
}
