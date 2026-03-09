using System.Security.Cryptography;
using System.Text;
using VehicleRental.Data;
using VehicleRental.Models;

namespace VehicleRental.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    private static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexStringLower(bytes);
    }

    public IEnumerable<User> GetAll() => _db.Users.ToList();

    public User? GetById(int id) => _db.Users.Find(id);

    public User? GetByUsername(string username) =>
        _db.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());

    public bool UsernameExists(string username) =>
        _db.Users.Any(u => u.Username.ToLower() == username.ToLower());

    public bool EmailExists(string email) =>
        _db.Users.Any(u => u.Email.ToLower() == email.ToLower());

    public void Add(User user)
    {
        user.PasswordHash = Hash(user.PasswordHash);
        _db.Users.Add(user);
        _db.SaveChanges();
    }
}
