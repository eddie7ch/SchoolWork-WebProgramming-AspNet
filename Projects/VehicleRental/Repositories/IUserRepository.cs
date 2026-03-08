using VehicleRental.Models;

namespace VehicleRental.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAll();
    User? GetById(int id);
    User? GetByUsername(string username);
    bool UsernameExists(string username);
    bool EmailExists(string email);
    void Add(User user);
}
