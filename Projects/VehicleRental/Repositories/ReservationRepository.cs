using VehicleRental.Models;

namespace VehicleRental.Repositories;

public class ReservationRepository : IReservationRepository
{
    private static readonly List<Reservation> _reservations =
    [
        new() { Id = 1, CustomerId = 1, VehicleId = 3, StartDate = DateTime.Today.AddDays(-5), EndDate = DateTime.Today.AddDays(-2), Status = ReservationStatus.Completed, Notes = "" },
        new() { Id = 2, CustomerId = 2, VehicleId = 1, StartDate = DateTime.Today,             EndDate = DateTime.Today.AddDays(3),  Status = ReservationStatus.Active,    Notes = "Airport pickup requested" },
        new() { Id = 3, CustomerId = 3, VehicleId = 2, StartDate = DateTime.Today.AddDays(2),  EndDate = DateTime.Today.AddDays(6),  Status = ReservationStatus.Pending,   Notes = "" },
        new() { Id = 4, CustomerId = 4, VehicleId = 5, StartDate = DateTime.Today.AddDays(-10),EndDate = DateTime.Today.AddDays(-7), Status = ReservationStatus.Cancelled, Notes = "Customer cancelled" },
    ];

    private static int _nextId = 5;

    public IEnumerable<Reservation> GetAll() => _reservations.ToList();

    public Reservation? GetById(int id) => _reservations.FirstOrDefault(r => r.Id == id);

    public void Add(Reservation reservation)
    {
        reservation.Id = _nextId++;
        _reservations.Add(reservation);
    }

    public void Update(Reservation reservation)
    {
        var existing = GetById(reservation.Id);
        if (existing is null) return;

        existing.CustomerId = reservation.CustomerId;
        existing.VehicleId  = reservation.VehicleId;
        existing.StartDate  = reservation.StartDate;
        existing.EndDate    = reservation.EndDate;
        existing.Status     = reservation.Status;
        existing.Notes      = reservation.Notes;
    }

    public void Delete(int id)
    {
        var reservation = GetById(id);
        if (reservation is not null) _reservations.Remove(reservation);
    }
}
