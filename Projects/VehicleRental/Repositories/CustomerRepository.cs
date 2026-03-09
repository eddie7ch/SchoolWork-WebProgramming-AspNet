using VehicleRental.Data;
using VehicleRental.Models;

namespace VehicleRental.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;
    public CustomerRepository(AppDbContext db) => _db = db;

    public IEnumerable<Customer> GetAll() => _db.Customers.ToList();

    public Customer? GetById(int id) => _db.Customers.Find(id);

    public void Add(Customer customer)
    {
        _db.Customers.Add(customer);
        _db.SaveChanges();
    }

    public void Update(Customer customer)
    {
        _db.Customers.Update(customer);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var customer = GetById(id);
        if (customer is null) return;
        _db.Customers.Remove(customer);
        _db.SaveChanges();
    }
}
