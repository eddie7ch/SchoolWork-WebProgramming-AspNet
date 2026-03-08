using VehicleRental.Models;

namespace VehicleRental.Repositories;

public interface IBillRepository
{
    IEnumerable<Bill> GetAll();
    Bill? GetById(int id);
    Bill? GetByReservationId(int reservationId);
    void Add(Bill bill);
    void Update(Bill bill);
    void Delete(int id);
}
