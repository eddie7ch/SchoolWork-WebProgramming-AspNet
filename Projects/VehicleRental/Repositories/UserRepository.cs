using System.Security.Cryptography;
using System.Text;
using VehicleRental.Models;

namespace VehicleRental.Repositories;

public class UserRepository : IUserRepository
{
    private static readonly List<User> _users =
    [
        new() { Id = 1, Username = "admin", Email = "admin@vrms.com", PasswordHash = Hash("Admin123!") },
    ];

    private static int _nextId = 2;

    private static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexStringLower(bytes);
    }

    public IEnumerable<User> GetAll() => _users.ToList();

    public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

    public User? GetByUsername(string username) =>
        _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

    public bool UsernameExists(string username) =>
        _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

    public bool EmailExists(string email) =>
        _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    public void Add(User user)
    {
        user.Id = _nextId++;
        user.PasswordHash = Hash(user.PasswordHash); // PasswordHash holds plain text before hashing
        _users.Add(user);
    }
}
