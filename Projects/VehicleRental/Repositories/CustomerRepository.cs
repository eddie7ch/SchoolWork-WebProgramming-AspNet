using VehicleRental.Models;

namespace VehicleRental.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private static readonly List<Customer> _customers =
    [
        new() { Id = 1, FirstName = "Alice",  LastName = "Johnson", Email = "alice@email.com",  Phone = "403-555-0101", LicenseNumber = "DL-10023", Address = "123 Main St, Calgary, AB" },
        new() { Id = 2, FirstName = "Bob",    LastName = "Smith",   Email = "bob@email.com",    Phone = "403-555-0202", LicenseNumber = "DL-20034", Address = "456 Oak Ave, Calgary, AB" },
        new() { Id = 3, FirstName = "Carol",  LastName = "Williams",Email = "carol@email.com",  Phone = "403-555-0303", LicenseNumber = "DL-30045", Address = "789 Elm Rd, Calgary, AB"  },
        new() { Id = 4, FirstName = "Daniel", LastName = "Brown",   Email = "daniel@email.com", Phone = "403-555-0404", LicenseNumber = "DL-40056", Address = "101 Pine Blvd, Calgary, AB" },
    ];

    private static int _nextId = 5;

    public IEnumerable<Customer> GetAll() => _customers.ToList();

    public Customer? GetById(int id) => _customers.FirstOrDefault(c => c.Id == id);

    public void Add(Customer customer)
    {
        customer.Id = _nextId++;
        _customers.Add(customer);
    }

    public void Update(Customer customer)
    {
        var existing = GetById(customer.Id);
        if (existing is null) return;

        existing.FirstName     = customer.FirstName;
        existing.LastName      = customer.LastName;
        existing.Email         = customer.Email;
        existing.Phone         = customer.Phone;
        existing.LicenseNumber = customer.LicenseNumber;
        existing.Address       = customer.Address;
    }

    public void Delete(int id)
    {
        var customer = GetById(id);
        if (customer is not null) _customers.Remove(customer);
    }
}
