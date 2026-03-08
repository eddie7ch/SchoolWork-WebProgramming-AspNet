using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using VehicleRental.Models;
using VehicleRental.Repositories;

namespace VehicleRental.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepo;

    public AccountController(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    // GET: /Account/Login
    public IActionResult Login() =>
        HttpContext.Session.GetString("Username") is not null
            ? RedirectToAction("Index", "Home")
            : View();

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var hash = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(model.Password)));
        var user = _userRepo.GetByUsername(model.Username);

        if (user is null || user.PasswordHash != hash)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetInt32("UserId", user.Id);
        return RedirectToAction("Index", "Home");
    }

    // GET: /Account/Register
    public IActionResult Register() =>
        HttpContext.Session.GetString("Username") is not null
            ? RedirectToAction("Index", "Home")
            : View();

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (_userRepo.UsernameExists(model.Username))
        {
            ModelState.AddModelError("Username", "Username is already taken.");
            return View(model);
        }

        if (_userRepo.EmailExists(model.Email))
        {
            ModelState.AddModelError("Email", "An account with this email already exists.");
            return View(model);
        }

        var user = new User
        {
            Username     = model.Username,
            Email        = model.Email,
            PasswordHash = model.Password  // UserRepository.Add() will hash it
        };
        _userRepo.Add(user);

        TempData["Success"] = "Registration successful. Please log in.";
        return RedirectToAction("Login");
    }

    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
